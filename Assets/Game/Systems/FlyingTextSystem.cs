using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class FlyingTextSystem : ECSSystem, ISystemInit, ISystemFixedUpdate
    {
        private IEnumerable<Entity> _filter;

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is AchiveTextComponent));
        }

        public void FixedUpdate()
        {
            foreach(var entity in _filter)
            {
                if (entity.Components.FirstOrDefault(c => c is AchiveTextComponent) is AchiveTextComponent component)
                {
                    component.Text.transform.position = Vector3.Lerp(component.Text.transform.position, component.TargetPosition, Time.fixedDeltaTime);
                }
            }
        }
    }
}
