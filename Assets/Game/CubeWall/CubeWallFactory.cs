
using Assets.Common.ECS;
using Assets.Game.Components;
using UnityEngine;

namespace Assets.Game.CubeWall
{
    public class CubeWallFactory : ICubeWallFactory
    {
        private RemovableGameObjectComponent _removableGameObjectComponent;
        private GameObject _cubeWall;
        private World _gameWorld;

        public CubeWallFactory(World world, GameObject cubeWall)
        {
            _cubeWall = cubeWall;
            _gameWorld = world;
        }

        public GameObject Create()
        {
            var cubeWall = Object.Instantiate(_cubeWall);

            var targetBlockEntity = _gameWorld.CreateEntity();
            targetBlockEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = cubeWall });
            _removableGameObjectComponent?.RelativeEntity?.Add(targetBlockEntity);

            return cubeWall;
        }

        public void SetRemovableComponent(RemovableGameObjectComponent removableGameObjectComponent)
        {
            _removableGameObjectComponent = removableGameObjectComponent;
        }
    }
}
