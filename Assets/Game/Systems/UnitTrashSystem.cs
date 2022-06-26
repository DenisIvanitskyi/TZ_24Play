using Assets.Common.ECS;
using Assets.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class UnitTrashSystem : ECSSystem, ISystemInit, ISystemUpdate, IDisposable
    {
        private IEnumerable<Entity> _filter;
        private HeroMovingComponent _heroComponent;

        public void Dispose()
        {
            var finalFilter = _filter.ToList();
            foreach (var entity in finalFilter)
            {
                var removableComponent = entity.Components.FirstOrDefault(c => c is RemovableGameObjectComponent) as RemovableGameObjectComponent;
                if (removableComponent != null)
                {
                    foreach (var rEntity in removableComponent.RelativeEntity)
                    {
                        World.RemoveEntity(rEntity);
                    }

                    entity.RemoveComponent(removableComponent);
                    World.RemoveEntity(entity);
                    GameObject.Destroy(removableComponent.GameObject, 0);
                }
            }
        }

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is RemovableGameObjectComponent));
            _heroComponent = World.Entities
                .Select(e => e.Components.FirstOrDefault(c => c is HeroMovingComponent))
                .FirstOrDefault(e => e != null) as HeroMovingComponent;
        }

        public void Update()
        {
            var finalFilter = _filter.ToList();
            foreach (var entity in finalFilter)
            {
                var removableComponent = entity.Components.FirstOrDefault(c => c is RemovableGameObjectComponent) as RemovableGameObjectComponent;
                if(removableComponent != null)
                {
                    var distance = Vector3.Distance(removableComponent.GameObject.transform.position, _heroComponent.Transform.position);
                    if (removableComponent.GameObject.transform.position.z < _heroComponent.Transform.position.z && distance >= 40)
                    {
                        foreach(var rEntity in removableComponent.RelativeEntity)
                        {
                            World.RemoveEntity(rEntity);
                        }

                        entity.RemoveComponent(removableComponent);
                        World.RemoveEntity(entity);
                        GameObject.Destroy(removableComponent.GameObject, 0);
                    }
                }
            }
        }
    }
}
