using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class RemovableGameObjectComponent : IComponent
    {
        public GameObject GameObject { get; set; }
    }
}
