using Assets.Common.Factory;
using Assets.Game.Components;
using UnityEngine;

namespace Assets.Game.PointCube
{
    public interface IPointCubeFactory : IFactory<GameObject>
    {
        void SetRemovableComponent(RemovableGameObjectComponent removableGameObjectComponent);
    }
}
