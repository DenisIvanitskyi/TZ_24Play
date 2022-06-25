using Assets.Common.ECS;
using Assets.Common.Factory;
using UnityEngine;

namespace Assets.Game.TrackGround
{
    public interface ITrackGroundFactory : IFactory<(GameObject, Entity)>
    {
    }
}
