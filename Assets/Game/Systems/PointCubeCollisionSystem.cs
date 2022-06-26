using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.Hero;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PointCubeCollisionSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private IEnumerable<Entity> _filter;
        private readonly IStackPointCube _stackPointCube;

        public PointCubeCollisionSystem(IStackPointCube stackPointCube)
        {
            _stackPointCube = stackPointCube;
        }

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is PointCubeCollisionComponent));
        }

        public void Update()
        {
            var finalList = _filter.ToList();
            foreach(var entity in finalList)
            {
                var pointCubeComponent = entity.Components.FirstOrDefault(c => c is PointCubeCollisionComponent) as PointCubeCollisionComponent;
                if(pointCubeComponent != null)
                {
                    var distance = Vector3.Distance(pointCubeComponent.PointCube.transform.position, pointCubeComponent.TargeteObject.transform.position);
                    if (distance < 1f)
                    {
                        World.RemoveEntity(entity);

                        var newEntity = World.CreateEntity();
                        newEntity.AddComponent(new HeroPointCubeComponent() { PointCube = pointCubeComponent.PointCube });

                        _stackPointCube.AddToStackCube(pointCubeComponent.PointCube);
                    }
                }
            }
        }
    }
}
