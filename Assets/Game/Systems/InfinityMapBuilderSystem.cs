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
            foreach (var entity in _filter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(c => c is HeroMovingComponent) as HeroMovingComponent;
                if (heroMovingComponent != null)
                {
                    CreateTrackGround(heroMovingComponent);
                }
            }
        }

        public void Update()
        {
            foreach(var entity in _filter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(c => c is HeroMovingComponent) as HeroMovingComponent;
                if(heroMovingComponent != null)
                {
                    CreateTrackGround(heroMovingComponent);
                }
            }
        }

        private void CreateTrackGround(HeroMovingComponent heroMovingComponent)
        {
            if (_lastTrackGround == null)
            {
                _lastTrackGround = _trackGroundFactory.Create();
                _lastTrackGround.transform.position = new Vector3();
            }

            var distance = Vector3.Distance(heroMovingComponent.Transform.position, _lastTrackGround.transform.position);
            if (distance <= 30 * 3)
            {
                var trackGround = _trackGroundFactory.Create();
                trackGround.transform.position = _lastTrackGround.transform.position + new Vector3(0, 0, _lastTrackGround.transform.localScale.z);
                _lastTrackGround = trackGround;
            }
        }
    }
}
