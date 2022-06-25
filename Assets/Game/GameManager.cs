using Assets.Common.Input;
using Assets.Game.UI;

namespace Assets.Game
{
    public class GameManager
    {
        private readonly IStartToGameSplashScreenController _startToGameSplashScreenController;
        private readonly Input _input;

        public GameManager(Input input, IStartToGameSplashScreenController startToGameSplashScreenController)
        {
            _input = input;
            _startToGameSplashScreenController = startToGameSplashScreenController;
        }

        public IStartToGameSplashScreenController StartSplashScreen => _startToGameSplashScreenController;

        public Input Input => _input;
    }
}
