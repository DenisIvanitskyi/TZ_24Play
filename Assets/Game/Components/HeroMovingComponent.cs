using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class HeroMovingComponent : IComponent
    {
        public Transform Transform { get; set; }

        public Vector3 TargetPosition { get; set; }
    }
}
