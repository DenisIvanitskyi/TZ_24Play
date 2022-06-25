using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class TrackGroundCreatingAnimationComponent : IComponent
    {
        public GameObject TrackGround { get; set; }

        public float SpeedAnimtion { get; set; }

        public Vector3 TargetPositon { get; set; }
    }
}
