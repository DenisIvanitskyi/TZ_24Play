using Assets.Common.ECS;
using Assets.Game.Components;
using UnityEngine;

namespace Assets.Game.PointCube
{
    public class CubePointFactory : IPointCubeFactory
    {
        private RemovableGameObjectComponent _removableGameObjectComponent;
        private GameObject _cubePoint;
        private World _gameWorld;
        private Transform _heroTrasform;

        public CubePointFactory(World world, GameObject cubePoint, Transform heroTrasform)
        {
            _cubePoint = cubePoint;
            _gameWorld = world;
            _heroTrasform = heroTrasform;
        }

        public GameObject Create()
        {
            var targetBlock = Object.Instantiate(_cubePoint);

            var targetBlockEntity = _gameWorld.CreateEntity();
            targetBlockEntity.AddComponent(new PointCubeCollisionComponent() { PointCube = targetBlock, TargeteObject = _heroTrasform });
            targetBlockEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = targetBlock });
            _removableGameObjectComponent?.RelativeEntity?.Add(targetBlockEntity);

            return targetBlock;
        }

        public void SetRemovableComponent(RemovableGameObjectComponent removableGameObjectComponent)
        {
            _removableGameObjectComponent = removableGameObjectComponent;
        }
    }
}
