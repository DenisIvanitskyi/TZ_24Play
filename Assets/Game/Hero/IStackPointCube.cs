using Assets.Common.ECS;
using System;
using UnityEngine;

namespace Assets.Game.Hero
{
    public interface IStackPointCube
    {
        public void Setup(World world, GameObject blowStackingEffect, GameObject achivePrefab, Camera uiCamera);

        void AddToStackCube(GameObject cube, bool withEffect = true, bool withAchiveText = true);

        void RemoveCubeFromStack(GameObject cube);

        void OnCubeStackEmpty(Action action);
    }
}
