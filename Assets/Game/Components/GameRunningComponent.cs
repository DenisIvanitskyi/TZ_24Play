
using Assets.Common.ECS;
using System;

namespace Assets.Game.Components
{
    public class GameRunningComponent : IComponent
    {
        public DateTime StartTime { get; set; }
    }
}
