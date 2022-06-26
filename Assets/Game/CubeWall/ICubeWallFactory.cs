using Assets.Common.Factory;
using Assets.Game.Components;
using UnityEngine;

namespace Assets.Game.CubeWall
{
    public interface ICubeWallFactory : IFactory<GameObject>
    {
        void SetRemovableComponent(RemovableGameObjectComponent removableGameObjectComponent);
    }
}
