using Assets.Common.ECS;
using Assets.Common.Factory;
using Assets.Game.Components;
using Assets.Game.CubeWall;
using Assets.Game.Hero;
using Assets.Game.PointCube;
using Assets.Game.Systems;
using Assets.Game.TrackGround;
using Assets.Game.UI;
using UnityEngine;

namespace Assets.Game
{
    public class GameBootstrapper : MonoBehaviour, ITrackGroundFactory
    {
        public static GameManager GameManager { get; private set; }

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _heroPrefab, _trackGround, _cubeWall, _cubePoint, _collectCubeText, _wrapEffect, _blowStackingEffect;

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

        public void Start()
        {
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
                .AddSystem(new HeroPointCubeWallDetectionSystem(_heroController))
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

            _heroController.Setup(_gameWorld, _blowStackingEffect);

            var heroEntity = _gameWorld.CreateEntity();
            heroEntity.AddComponent(new HeroMovingComponent() { Transform = _heroObject.transform });

            var cubeObject = Instantiate(_cubePoint);

            var cubeEntity = _gameWorld.CreateEntity();
            cubeEntity.AddComponent(new HeroPointCubeComponent() { PointCube = cubeObject });
            cubeEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = cubeObject });
            _heroController.AddToStackCube(cubeObject, false);
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
