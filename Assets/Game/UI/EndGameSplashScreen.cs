
using System;
using UnityEngine;

namespace Assets.Game.UI
{
    public class EndGameSplashScreen : MonoBehaviour
    {
        public Action OnTryAgaineAction { get; set; }

        public void OnTryAgainGame()
        {
            OnTryAgaineAction?.Invoke();
        }
    }
}
