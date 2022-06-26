using Assets.Common.ECS;
using Assets.Game.Components;
using Assets.Game.Hero;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class HeroPointCubeWallDetectionSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private IEnumerable<Entity> _filter;
        private IEnumerable<Entity> _wallCubes;
        private IStackPointCube _stackPointCube;
        private bool _isGameStarted;

        public HeroPointCubeWallDetectionSystem(IStackPointCube stackPointCube)
        {
            _stackPointCube = stackPointCube;
        }

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is HeroPointCubeComponent));
            _wallCubes = World.Entities.Where(e => e.Components.Any(c => c is CubeWallComponent));
        }

        public void Update()
        {
            if (!_isGameStarted)
                _isGameStarted = World.Entities.Any(e => e.Components.Any(c => c is GameRunningComponent));
            if (!_isGameStarted) return;

            var finalFilter = _filter.ToList();
            var finalWallCubes = _wallCubes.ToList();
            foreach (var entity in finalFilter)
            {
                var pointCubeComponent = entity.Components.FirstOrDefault(c => c is HeroPointCubeComponent) as HeroPointCubeComponent;
                if (pointCubeComponent != null)
                {
                    foreach(var wallCube in finalWallCubes)
                    {
                        var wallCubeComponent = wallCube.Components.FirstOrDefault(c => c is CubeWallComponent) as CubeWallComponent;
                        if(wallCubeComponent != null)
                        {
                            var distanceZ = Mathf.Abs(wallCubeComponent.CubeWall.transform.position.z - pointCubeComponent.PointCube.transform.position.z);
                            var distanceY = Mathf.Abs(wallCubeComponent.CubeWall.transform.position.y - pointCubeComponent.PointCube.transform.position.y);
                            var distanceX = Mathf.Abs(wallCubeComponent.CubeWall.transform.position.x - pointCubeComponent.PointCube.transform.position.x);
                            if (distanceZ < 1f && distanceX <= 0.5f && distanceY <= 0.5f)
                            {
                                var rayableDirection = pointCubeComponent.PointCube.transform.TransformDirection(Vector3.forward);
                                if (Physics.Raycast(pointCubeComponent.PointCube.transform.position, rayableDirection, 2))
                                {
                                    entity.RemoveComponent();
                                    entity.AddComponent(new RemovableGameObjectComponent() { GameObject = pointCubeComponent.PointCube });
                                    _stackPointCube.RemoveCubeFromStack(pointCubeComponent.PointCube);
                                    break;
                                }
                            }
                        }
                    }
                    
                }
            }
        }
    }
}
