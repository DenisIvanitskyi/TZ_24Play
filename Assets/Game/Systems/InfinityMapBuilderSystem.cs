using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.PointCube;
using Assets.Game.TrackGround;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Game.Systems
{
    public class InfinityMapBuilderSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private readonly ITrackGroundFactory _trackGroundFactory;
        private readonly IPointCubeFactory _pointCubeFactory;

        private IEnumerable<Entity> _filter;
        private Vector3 _nextPosition;

        public InfinityMapBuilderSystem(ITrackGroundFactory trackGroundFactory, IPointCubeFactory pointCubeFactory)
        {
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
                    for (var i = 0; i <= 2; i++)
                        CreateTrackGround(heroMovingComponent, true);
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

        private bool CreateTrackGround(HeroMovingComponent heroMovingComponent, bool isInit = false)
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

                _nextPosition += new Vector3(0, 0, trackGround.transform.localScale.z);
                GeneratePointBlock(trackGround);

                return true;
            }

            return false;
        }

        private void GeneratePointBlock(GameObject gameObject)
        {
            for (var i = 0; i < 3; i++)
            {
                var pointBlock = _pointCubeFactory.Create();
                pointBlock.transform.SetParent(gameObject.transform);

                var randomPositon = new Vector3((int)Math.Floor((decimal)Random.Range(-2, 3)), gameObject.transform.position.y + .5f, gameObject.transform.position.z + 8 + i * 6);
                pointBlock.transform.position = randomPositon;
            }
        }
    }
}
