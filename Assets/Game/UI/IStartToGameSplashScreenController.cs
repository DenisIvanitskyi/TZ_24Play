using Assets.Common.UI;

namespace Assets.Game.UI
{
    public interface IStartToGameSplashScreenController : IPrimaryUIController
    {
        bool IsRunningAnimationOfPointer { get; set; }

        float PointerAnimtionSpeed { get; set; }
    }
}
