using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class CameraFollowingComponent : IComponent
    {
        public Camera Camera { get; set; }

        public Transform Target { get; set; }

        public Vector3 Offset { get; set; }
    }
}
