﻿using Assets.Common.ECS;
using Assets.Common.Factory;
using Assets.Game.Components;
using Assets.Game.Hero;
using Assets.Game.PointCube;
using Assets.Game.Systems;
using Assets.Game.TrackGround;
using Assets.Game.UI;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game
{
    public class GameBootstrapper : MonoBehaviour, ITrackGroundFactory, IPointCubeFactory
    {
        public static GameManager GameManager { get; private set; }

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _heroPrefab, _trackGround, _cubeWall, _cubePoint, _collectCubeText;

        [Header("Instances - UI")]
        [SerializeField]
        private StartToGameSplashScreen _splashStartGameScreen;

        [Header("Input")]
        [SerializeField]
        private Common.Input.Input _input;

        private World _gameWorld;
        private GameObject _heroObject;
        private HeroController _heroController;
        private RemovableGameObjectComponent _removableGameObjectComponent;

        public void Start()
        {
            _splashStartGameScreen.IsVisible = true;
            GameManager = new GameManager(_input, _splashStartGameScreen);

            _gameWorld = new World();

            CreateSplashScreenEntity();
            CreatePlayer();
            CreateCamera(Camera.main, _heroObject.transform);

            _gameWorld
                .AddSystem(new GameStartingSystem(_input))
                .AddSystem(new HeroInputSystem(_input))
                .AddSystem(new HeroMovingSystem())
                .AddSystem(new CameraTargetFollowingSystem())
                .AddSystem(new InfinityMapBuilderSystem(this, this))
                .AddSystem(new PointCubeCollisionSystem(_heroController))
                .AddSystem(new TruckGroundAnimationSystem())
                .AddSystem(new UnitTrashSystem());

            _gameWorld.Init();
        }

        public void Update()
        {
            _gameWorld?.Update();
        }

        public void FixedUpdate()
        {
            _gameWorld?.FixedUpdate();
        }

        private void CreateCamera(Camera camera, Transform heroTransform)
        {
            var cameraEntity = _gameWorld.CreateEntity(nameof(Camera));
            cameraEntity.AddComponent(new CameraFollowingComponent() { Target = heroTransform, Camera = camera });
        }

        private void CreatePlayer()
        {
            _heroObject = Instantiate(_heroPrefab);
            _heroController = _heroObject.GetComponent<HeroController>();
            _heroObject.transform.position = new Vector3(0, 0, 5);

            var heroEntity = _gameWorld.CreateEntity();
            heroEntity.AddComponent(new HeroMovingComponent() { Transform = _heroObject.transform });
        }

        private void CreateSplashScreenEntity()
        {
            var splashEntity = _gameWorld.CreateEntity();
            splashEntity.AddComponent(new GameSplashScreenComponent() { StartToGameSplashScreenController = _splashStartGameScreen });
        }

        public (GameObject, Entity) Create()
        {
            var trackGround = Instantiate(_trackGround);

            var trackGroundEntity = _gameWorld.CreateEntity();
            trackGroundEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = trackGround });
            return (trackGround, trackGroundEntity);
        }

        GameObject IFactory<GameObject>.Create()
        {
            var targetBlock = Instantiate(_cubePoint);

            var targetBlockEntity = _gameWorld.CreateEntity();
            targetBlockEntity.AddComponent(new PointCubeCollisionComponent() { PointCube = targetBlock, TargeteObject = _heroObject.transform });
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
