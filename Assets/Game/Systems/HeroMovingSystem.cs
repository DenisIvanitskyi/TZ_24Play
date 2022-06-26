using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class HeroMovingSystem : ECSSystem, ISystemInit, ISystemFixedUpdate
    {
        private IEnumerable<Entity> _filter;
        private bool _isGameRunning;

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is HeroMovingComponent));
        }

        public void FixedUpdate()
        {
            if (!_isGameRunning)
                _isGameRunning = World.Entities.Any(e => e.Components.Any(c => c is GameRunningComponent));
            if (!_isGameRunning) return;

            foreach (var entity in _filter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(e => e is HeroMovingComponent) as HeroMovingComponent;
                if (heroMovingComponent != null)
                {
                    var nextPostion = heroMovingComponent.Transform.position + Vector3.forward;
                    var newPosition 
                        = Vector3.MoveTowards(heroMovingComponent.Transform.position, nextPostion, Time.fixedDeltaTime * 5f);
                    heroMovingComponent.Transform.position = new Vector3(heroMovingComponent.Transform.position.x, heroMovingComponent.Transform.position.y, newPosition.z);
                }
            }
        }
    }
}
