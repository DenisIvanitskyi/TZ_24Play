using Assets.Common.ECS;
using Assets.Game.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Hero
{
    public class HeroController : MonoBehaviour, IStackPointCube
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private GameObject _cubeHolder;

        private List<GameObject> _stack = new List<GameObject>();
        private Action _onCubeStackEmpty;
        private World _world;
        private GameObject _blowStackingEffect;

        public void Start()
        {
            _player.SetActive(false);
            var rigibBodies = _player.GetComponentsInChildren<Rigidbody>();
            foreach(var rigidBody in rigibBodies)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
            }
        }

        public void AddToStackCube(GameObject cube, bool withEffect = true)
        {
            var rigidBody = cube.GetComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;

            cube.transform.SetParent(_cubeHolder.transform);
            var newCubePosition =
                new Vector3(_cubeHolder.transform.position.x, _cubeHolder.transform.position.y + cube.transform.localScale.y * _stack.Count, _cubeHolder.transform.position.z);

            _player.transform.position = new Vector3(newCubePosition.x, newCubePosition.y + cube.transform.localScale.y, newCubePosition.z);
            cube.transform.position = newCubePosition;
            _stack.Add(cube);

            if (withEffect)
            {
                var blowStackingEffectGameObject = Instantiate(_blowStackingEffect);
                blowStackingEffectGameObject.transform.position = newCubePosition;

                var blowStakingRemoveEntity = _world.CreateEntity();
                blowStakingRemoveEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = blowStackingEffectGameObject });
            }

            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }

        public void OnCubeStackEmpty(Action action)
        {
            _onCubeStackEmpty = action;
        }

        public void RemoveCubeFromStack(GameObject cube)
        {
            cube.transform.SetParent(null);

            var rigidBody = cube.GetComponent<Rigidbody>();
            if(rigidBody != null)
                Destroy(rigidBody);

            _stack.Remove(cube);    
            if (_stack.Count <= 0)
                _onCubeStackEmpty();
        }

        public void Setup(World world, GameObject gameObject)
        {
            _world = world;
            _blowStackingEffect = gameObject;
        } 
    }
}
