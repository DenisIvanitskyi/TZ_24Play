using Assets.Common.Base;
using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.CubeWall;
using Assets.Game.PointCube;
using Assets.Game.TrackGround;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Game.Systems
{
    public class InfinityMapBuilderSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private readonly ITrackGroundFactory _trackGroundFactory;
        private readonly IPointCubeFactory _pointCubeFactory;
        private readonly ICubeWallFactory _cubeWallFactory;

        private IEnumerable<Entity> _filter;
        private Vector3 _nextPosition;

        public InfinityMapBuilderSystem(ITrackGroundFactory trackGroundFactory, IPointCubeFactory pointCubeFactory, ICubeWallFactory cubeWallFactory)
        {
            _cubeWallFactory = cubeWallFactory;
            _trackGroundFactory = trackGroundFactory;
            _pointCubeFactory = pointCubeFactory;
            _nextPosition = new Vector3(0, 0, 0);
        }

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is HeroMovingComponent));
            var finalFilter = _filter.ToList();
            foreach (var entity in finalFilter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(c => c is HeroMovingComponent) as HeroMovingComponent;
                if (heroMovingComponent != null)
                {
                    CreateTrackGround(heroMovingComponent, true, true);
                    CreateTrackGround(heroMovingComponent, true, true);
                }
            }
        }

        public void Update()
        {
            var isGameStart = World.Entities.Any(e => e.Components.Any(c => c is GameRunningComponent));
            if (!isGameStart) return;

            var finalFilter = _filter.ToList();
            foreach (var entity in finalFilter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(c => c is HeroMovingComponent) as HeroMovingComponent;
                if (heroMovingComponent != null)
                {

                    CreateTrackGround(heroMovingComponent);
                }
            }
        }

        private bool CreateTrackGround(HeroMovingComponent heroMovingComponent, bool isInit = false, bool spawnPointCube = true)
        {
            var distance = _nextPosition.z - heroMovingComponent.Transform.position.z;
            if (distance <= 35 || isInit)
            {
                var tuple = _trackGroundFactory.Create();
                var trackGround = tuple.Item1;
                if (!isInit)
                {
                    tuple.Item2.AddComponent(new TrackGroundCreatingAnimationComponent() { SpeedAnimtion = 5f, TargetPositon = _nextPosition, TrackGround = trackGround });
                    trackGround.transform.position = new Vector3(0, -100, _nextPosition.z);
                }
                else
                    trackGround.transform.position = _nextPosition;

                var removableGOComponent
                    = tuple.Item2.Components.FirstOrDefault(c => c is RemovableGameObjectComponent) as RemovableGameObjectComponent;
                if (spawnPointCube)
                    GeneratePointBlock(trackGround, removableGOComponent);
                if (spawnPointCube)
                    GenerateCubeWall(trackGround, removableGOComponent);

                _nextPosition += new Vector3(0, 0, trackGround.transform.localScale.z);
                return true;
            }

            return false;
        }

        private void GeneratePointBlock(GameObject gameObject, RemovableGameObjectComponent removableGameObjectComponent)
        {
            for (var i = 0; i < 3; i++)
            {
                _pointCubeFactory.SetRemovableComponent(removableGameObjectComponent);
                var pointBlock = _pointCubeFactory.Create();
                pointBlock.transform.SetParent(gameObject.transform);

                var randomPositon = new Vector3((int)Math.Floor((decimal)Random.Range(-2, 3)), gameObject.transform.position.y + .5f, gameObject.transform.position.z + 8 + i * 6);
                pointBlock.transform.position = randomPositon;
            }
        }

        private void GenerateCubeWall(GameObject gameObject, RemovableGameObjectComponent removableGameObjectComponent)
        {
            var perlineNoise = new PerlinNoise(5, 3, 0, 3);
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var cubeExist = perlineNoise.NoiseValueAt(x, y) > (float)Random.Range(0.35f, 0.45f);
                    if (!cubeExist) continue;

                    _cubeWallFactory.SetRemovableComponent(removableGameObjectComponent);
                    var cubeWall = _cubeWallFactory.Create();
                    cubeWall.transform.SetParent(gameObject.transform);

                    var randomPositon = new Vector3(gameObject.transform.position.x + x - 2, gameObject.transform.position.y + y, gameObject.transform.position.z + 29);
                    cubeWall.transform.position = randomPositon;
                }
            }
        }
    }
}
