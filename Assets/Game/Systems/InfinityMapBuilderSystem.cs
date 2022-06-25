using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.TrackGround;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class InfinityMapBuilderSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private readonly ITrackGroundFactory _trackGroundFactory;
        private IEnumerable<Entity> _filter;
        private GameObject _lastTrackGround;

        public InfinityMapBuilderSystem(ITrackGroundFactory trackGroundFactory)
        {
            _trackGroundFactory = trackGroundFactory;
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
                    for (var i = 0; i <= 2; i++)
                        CreateTrackGround(heroMovingComponent);
                }
            }
        }

        private bool CreateTrackGround(HeroMovingComponent heroMovingComponent, bool isInit = false)
        {
            if (_lastTrackGround == null)
            {
                var tuple = _trackGroundFactory.Create();
                _lastTrackGround = tuple.Item1;
                _lastTrackGround.transform.position = new Vector3();
            }

            var distance = Vector3.Distance(heroMovingComponent.Transform.position, _lastTrackGround.transform.position);
            if (distance <= 30)
            {
                var tuple = _trackGroundFactory.Create();
                var trackGround = tuple.Item1;
                var finalPosition = _lastTrackGround.transform.position + new Vector3(0, 0, _lastTrackGround.transform.localScale.z);
                if (!isInit)
                {
                    tuple.Item2.AddComponent(new TrackGroundCreatingAnimationComponent() { SpeedAnimtion = 5f, TargetPositon = finalPosition, TrackGround = trackGround });
                    trackGround.transform.position = new Vector3(0, -100, finalPosition.z);
                }
                else
                    trackGround.transform.position = finalPosition;
                _lastTrackGround = trackGround;

                return true;
            }

            return false;
        }
    }
}
