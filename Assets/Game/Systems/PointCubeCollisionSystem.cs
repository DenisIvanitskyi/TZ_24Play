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
                    var distanceZ = Mathf.Abs(pointCubeComponent.PointCube.transform.position.z - pointCubeComponent.TargeteObject.position.z);
                    if(distanceZ < 1)
                    {
                        var distanceX = Mathf.Abs(pointCubeComponent.PointCube.transform.position.x - pointCubeComponent.TargeteObject.position.x);
                        if(distanceX < 1f)
                        {
                            entity.RemoveComponent(pointCubeComponent);
                            World.RemoveEntity(entity);

                            _stackPointCube.AddToStackCube(pointCubeComponent.PointCube);
                        }
                    }
                }
            }
        }
    }
}
