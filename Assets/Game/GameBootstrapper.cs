using Assets.Common.ECS;
using Assets.Common.Factory;
using Assets.Game.Components;
using Assets.Game.CubeWall;
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
    public class GameBootstrapper : MonoBehaviour, ITrackGroundFactory
    {
        public static GameManager GameManager { get; private set; }

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _heroPrefab, _trackGround, _cubeWall, _cubePoint, _collectCubeText, _wrapEffect, _blowStackingEffect;

        [Header("Instances - Camera")]
        [SerializeField]
        private Camera _uiCamera;

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

        private ICubeWallFactory _cubeWallFactory;
        private IPointCubeFactory _pointCubeFactory;
        private bool _isGameEnd;

        public void Start()
        {
            _isGameEnd = false;
            _splashStartGameScreen.IsVisible = true;
            GameManager = new GameManager(_input, _splashStartGameScreen);

            _gameWorld = new World();

            CreateSplashScreenEntity();
            CreatePlayer();
            CreateCamera(Camera.main, _heroObject.transform);

            _cubeWallFactory = new CubeWallFactory(_gameWorld, _cubeWall);
            _pointCubeFactory = new CubePointFactory(_gameWorld, _cubePoint, _heroObject.transform);

            _gameWorld
                .AddSystem(new GameStartingSystem(_input, _wrapEffect, _heroController.transform))
                .AddSystem(new HeroInputSystem(_input))
                .AddSystem(new HeroMovingSystem())
                .AddSystem(new CameraTargetFollowingSystem())
                .AddSystem(new InfinityMapBuilderSystem(this, _pointCubeFactory, _cubeWallFactory))
                .AddSystem(new PointCubeCollisionSystem(_heroController))
                .AddSystem(new FlyingTextSystem())
                .AddSystem(new HeroPointCubeWallDetectionSystem(_heroController))
                .AddSystem(new TruckGroundAnimationSystem())
                .AddSystem(new UnitTrashSystem());

            _gameWorld.Init();
        }

        public void Update()
        {
            if (_isGameEnd) return;

            _gameWorld?.Update();
        }

        public void FixedUpdate()
        {
            if (_isGameEnd) return;

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

            _heroController.Setup(_gameWorld, _blowStackingEffect, _collectCubeText, _uiCamera);
            _heroController.OnCubeStackEmpty(OnGameEnd);

            var heroEntity = _gameWorld.CreateEntity();
            heroEntity.AddComponent(new HeroMovingComponent() { Transform = _heroObject.transform });

            var cubeObject = Instantiate(_cubePoint);

            var cubeEntity = _gameWorld.CreateEntity();
            cubeEntity.AddComponent(new HeroPointCubeComponent() { PointCube = cubeObject });
            cubeEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = cubeObject });
            _heroController.AddToStackCube(cubeObject, false, false);
        }

        private void OnGameEnd()
        {
            _isGameEnd = true;
            var wrapEffect = _heroController.transform.Find("WrapEffect");
            if (wrapEffect != null)
            {
                wrapEffect.SetParent(null);
                Destroy(wrapEffect.gameObject);
            }    
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

        public void SetRemovableComponent(RemovableGameObjectComponent removableGameObjectComponent)
        {
            _removableGameObjectComponent = removableGameObjectComponent;
        }
    }
}
