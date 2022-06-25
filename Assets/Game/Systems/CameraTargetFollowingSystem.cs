using Assets.Common.ECS;
using Assets.Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class CameraTargetFollowingSystem : ECSSystem, ISystemInit, ISystemFixedUpdate
    {
        private IEnumerable<Entity> _filter;
        private Vector3 currentVelocity;

        private Vector3 _offset = new Vector3(0, 0, -6);

        public void Init()
        {
            _filter = 
                World.Entities.Where(e => e.Components.Any(c => c is CameraFollowingComponent));
        }

        public void FixedUpdate()
        {
            foreach (var entity in _filter)
            {
                var component = entity.Components.FirstOrDefault(c => c is CameraFollowingComponent) as CameraFollowingComponent;
                if (component != null)
                {
                    var camPosition = component.Camera.transform.position;
                    var newPosition
                        = Vector3.SmoothDamp(camPosition, component.Target.transform.position, ref currentVelocity, Time.fixedDeltaTime);
                    component.Camera.transform.position = new Vector3(camPosition.x, camPosition.y, newPosition.z + _offset.z);
                }
            }
        }
    }
}
