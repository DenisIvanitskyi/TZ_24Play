using Assets.Common.ECS;
using Assets.Game.UI;
using UnityEngine;

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

        private World _gameWorld;

        public void Start()
        {
            GameManager = new GameManager(_splashStartGameScreen);

            _gameWorld = new World();


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
    }
}
