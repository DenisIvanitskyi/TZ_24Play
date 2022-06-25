using Assets.Common.ECS;
using Assets.Game.UI;
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

            _gameWorld = new World();


            _gameWorld.Init();

            GameManager.Input.SubscibeOnTap(StartGame);
        }

        public void Update()
        {
            _gameWorld?.Update();
        }

        public void FixedUpdate()
        {
            _gameWorld?.FixedUpdate();
        }

        private void StartGame(InputAction.CallbackContext ctx)
        {
            GameManager.Input.UnsubscribeTap(StartGame);
            GameManager.StartSplashScreen.IsVisible = false;
            Debug.Log("Tap");
        }
    }
}
