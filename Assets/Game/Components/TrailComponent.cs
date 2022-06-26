using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class TrailComponent : IComponent
    {
        public TrailRenderer TrailRenderer { get; set; }
    }
}
