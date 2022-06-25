using Assets.Game.UI;

namespace Assets.Game
{
    public class GameManager
    {
        private readonly IStartToGameSplashScreenController _startToGameSplashScreenController;

        public GameManager(IStartToGameSplashScreenController startToGameSplashScreenController)
        {
            _startToGameSplashScreenController = startToGameSplashScreenController;
        }

        public IStartToGameSplashScreenController StartSplashScreen => _startToGameSplashScreenController;
    }
}
