using Assets.Common.ECS;
using System;
using UnityEngine;

namespace Assets.Game.Hero
{
    public interface IStackPointCube
    {
        public void Setup(World world, GameObject blowStackingEffect);

        void AddToStackCube(GameObject cube, bool withEffect = true);

        void RemoveCubeFromStack(GameObject cube);

        void OnCubeStackEmpty(Action action);
    }
}
