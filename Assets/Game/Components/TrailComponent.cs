using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class TrailComponent : IComponent
    {
        public GameObject TrailPrefab { get; set; }

        public Vector3 LastPoint { get; set; }

        public Transform HeroTransform { get; internal set; }
    }
}
