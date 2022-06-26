using Assets.Common.ECS;
using UnityEngine;

namespace Assets.Game.Components
{
    public class PointCubeCollisionComponent : IComponent
    {
        public Transform TargeteObject { get; set; }

        public GameObject PointCube { get; set; }
    }
}
