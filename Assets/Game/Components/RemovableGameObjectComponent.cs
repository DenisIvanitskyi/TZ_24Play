using Assets.Common.ECS;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Components
{
    public class RemovableGameObjectComponent : IComponent
    {
        public GameObject GameObject { get; set; }

        public List<Entity> RelativeEntity { get; set; } = new List<Entity>();
    }
}
