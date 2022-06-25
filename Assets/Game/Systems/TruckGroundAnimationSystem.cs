using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class TruckGroundAnimationSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private IEnumerable<Entity> _filter;

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is TrackGroundCreatingAnimationComponent));
        }

        public void Update()
        {
            var finalFilter = _filter.ToList();
            foreach (var entity in finalFilter)
            {
                var trackCreationAnimationComponent 
                    = entity.Components.FirstOrDefault(c => c is TrackGroundCreatingAnimationComponent) as TrackGroundCreatingAnimationComponent;
                if(trackCreationAnimationComponent != null)
                {
                    trackCreationAnimationComponent.TrackGround.transform.position
                        = Vector3.LerpUnclamped(trackCreationAnimationComponent.TrackGround.transform.position,
                            trackCreationAnimationComponent.TargetPositon, Time.deltaTime * trackCreationAnimationComponent.SpeedAnimtion);
                    if(Vector3.Distance(trackCreationAnimationComponent.TrackGround.transform.position, trackCreationAnimationComponent.TargetPositon) <= 0.01f)
                    {
                        trackCreationAnimationComponent.TrackGround.transform.position = trackCreationAnimationComponent.TargetPositon;
                        entity.RemoveComponent(trackCreationAnimationComponent);
                    }
                }
            }
        }
    }
}
