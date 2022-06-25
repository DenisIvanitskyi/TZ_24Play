﻿using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.Systems;
using Assets.Game.TrackGround;
using Assets.Game.UI;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game
{
    public class GameBootstrapper : MonoBehaviour, ITrackGroundFactory
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

        public void Start()
        {
            _splashStartGameScreen.IsVisible = true;
            GameManager = new GameManager(_input, _splashStartGameScreen);

            _gameWorld = new World()
                .AddSystem(new GameStartingSystem(_input))
                .AddSystem(new HeroInputSystem(_input))
                .AddSystem(new HeroMovingSystem())
                .AddSystem(new CameraTargetFollowingSystem())
                .AddSystem(new InfinityMapBuilderSystem(this))
                .AddSystem(new UnitTrashSystem());

            CreateTrashEntity();
            CreateSplashScreenEntity();
            var hero = CreatePlayer();
            CreateCamera(Camera.main, hero.transform);

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

        private GameObject CreatePlayer()
        {
            var heroObject = Instantiate(_heroPrefab);
            heroObject.transform.position = new Vector3(0, 0, 4);

            var heroEntity = _gameWorld.CreateEntity();
            heroEntity.AddComponent(new HeroMovingComponent() { Transform = heroObject.transform });

            return heroObject;
        }

        private void CreateSplashScreenEntity()
        {
            var splashEntity = _gameWorld.CreateEntity();
            splashEntity.AddComponent(new GameSplashScreenComponent() { StartToGameSplashScreenController = _splashStartGameScreen });
        }

        private void CreateTrashEntity()
        {
            _gameWorld.CreateEntity("Trash");
        }

        public GameObject Create()
        {
            var trackGround = Instantiate(_trackGround);

            var worldEntity = _gameWorld.Entities.FirstOrDefault(e => e.Name == "Trash");
            worldEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = trackGround });

            return trackGround;
        }
    }
}
