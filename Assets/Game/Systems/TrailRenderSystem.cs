using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    internal class TrailRenderSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private IEnumerable<Entity> _filter;

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is TrailComponent));
        }

        public void Update()
        {
            foreach(var entity in _filter)
            {
                var component = entity.Components.FirstOrDefault(c => c is TrailComponent) as TrailComponent;
                if(component != null)
                {
                    var distance = Vector3.Distance(component.HeroTransform.position, component.LastPoint);
                    for(var i = 0; i < distance; i++)
                    {
                        var newTrail = GameObject.Instantiate(component.TrailPrefab);
                        component.LastPoint = Vector3.MoveTowards(component.HeroTransform.position, component.LastPoint, Time.deltaTime);

                        var trailEntity = World.CreateEntity();
                        trailEntity.AddComponent(new RemovableGameObjectComponent() { GameObject = newTrail });
                    }
                }
            }
        }
    }
}
