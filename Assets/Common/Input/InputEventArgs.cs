using System;
using UnityEngine.InputSystem;

namespace Assets.Common.Input
{
    public class InputEventArgs : EventArgs
    {
        public InputEventArgs(InputAction.CallbackContext context)
        {
            Context = context;
        }

        public InputAction.CallbackContext Context { get; }
    }

    public class TapInputEventArgs : InputEventArgs
    {
        public TapInputEventArgs(InputAction.CallbackContext context, bool isTapStarted) : base(context)
        {
            IsTapStarted = isTapStarted;
        }

        public bool IsTapStarted { get; }

        public bool IsTapEnded => !IsTapStarted;
    }

    public class SwipeInputEventArgs : InputEventArgs
    {
        public enum SwipeDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public SwipeInputEventArgs(InputAction.CallbackContext startContext, InputAction.CallbackContext endContext, SwipeDirection swipe) : base(endContext)
        {
            StartContext = startContext;
            Swipe = swipe;
        }   

        public SwipeDirection Swipe { get; }

        public InputAction.CallbackContext StartContext { get; }

        public InputAction.CallbackContext EndContext => Context;
    }
}
