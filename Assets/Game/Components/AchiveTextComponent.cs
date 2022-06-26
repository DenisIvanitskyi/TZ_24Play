using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class AchiveTextComponent : IComponent
    {
        public Vector3 TargetPosition { get; set; }

        public GameObject Text { get; set; }
    }
}
