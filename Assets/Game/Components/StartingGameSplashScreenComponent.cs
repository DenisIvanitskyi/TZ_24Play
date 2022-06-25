using Assets.Common.ECS;
using Assets.Game.UI;

namespace Assets.Game.Components
{
    public class GameSplashScreenComponent : IComponent
    {
        public IStartToGameSplashScreenController StartToGameSplashScreenController { get; set; } 
    }
}
