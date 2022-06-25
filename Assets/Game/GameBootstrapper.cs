using Assets.Common.ECS;
using Assets.Common.Input;
using Assets.Game.Components;
using Assets.Game.Systems;
using Assets.Game.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Game
{
    public class GameBootstrapper : MonoBehaviour
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
                .AddSystem(new HeroMovingSystem());

            CreateSplashScreenEntity();


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

        private void StartGame()
        {
            GameManager.StartSplashScreen.IsVisible = false;
            Debug.Log("Tap");
        }

        private void CreatePlayer()
        {
            var heroObject = Instantiate(_heroPrefab);
            heroObject.transform.position = new Vector3(0, 0, 30);

            var heroEntity = _gameWorld.CreateEntity();
            heroEntity.AddComponent(new HeroMovingComponent() { Transform = heroObject.transform });
        }

        private void CreateSplashScreenEntity()
        {
            var splashEntity = _gameWorld.CreateEntity();
            splashEntity.AddComponent(new GameSplashScreenComponent() { StartToGameSplashScreenController = _splashStartGameScreen });
        }
    }
}
