using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Systems
{
    public class HeroMovingSystem : ECSSystem, ISystemInit, ISystemFixedUpdate
    {
        private IEnumerable<Entity> _filter;

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is HeroMovingComponent));
        }

        public void FixedUpdate()
        {
            foreach(var entity in _filter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(e => e is HeroMovingComponent) as HeroMovingComponent;
                if(heroMovingComponent != null)
                {

                }
            }
        }
    }
}
