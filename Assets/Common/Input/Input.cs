using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Common.Input
{
    public class Input : MonoBehaviour
    {
        private InputControls _inputControls;
        private InputAction.CallbackContext _endContext, _startContext;

        private Vector2 _startPosition, _endPosition;
        private double _startTime, _endTime;

        private float minmumDistance = 0.2f;
        private double maximumTime = 1;
        private float directionThreshold = 0.9f;

        public event EventHandler<InputEventArgs> OnInput;

        private void Awake()
        {
            if (_inputControls == null)
                _inputControls = new InputControls();
        }

        private void Start()
        {
            _inputControls.Touch.Tap.started += TapStarted;
            _inputControls.Touch.Tap.canceled += TapEnd;

            _inputControls.Touch.PrimaryContact.started += PrimaryContactStarted;
            _inputControls.Touch.PrimaryContact.canceled += PrimaryContactCanceled;
        }

        private void PrimaryContactStarted(InputAction.CallbackContext obj)
        {
            _startContext = obj;
            _startPosition = ScreenToWorld(Camera.main, _inputControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            _startTime = obj.startTime;
        }

        private void PrimaryContactCanceled(InputAction.CallbackContext obj)
        {
            _endContext = obj;
            _endPosition = ScreenToWorld(Camera.main, _inputControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            _endTime = obj.startTime;
            DetectSwipe();
        }


        private void OnEnable() => _inputControls?.Enable();

        private void OnDisabled() => _inputControls?.Disable();

        private void TapEnd(InputAction.CallbackContext obj)
        {
            OnInput?.Invoke(this, new TapInputEventArgs(obj, false));
        }

        private void TapStarted(InputAction.CallbackContext obj)
        {
            OnInput?.Invoke(this, new TapInputEventArgs(obj, true));
        }

        private void DetectSwipe()
        {
            if (Vector3.Distance(_startPosition, _endPosition) >= minmumDistance && (_endTime - _startTime) <= maximumTime)
            {
                var direction = _endPosition - _startPosition;
                var direction2D = new Vector2(direction.x, direction.y).normalized;
                SwipeDirection(direction2D);
            }
        }

        private void SwipeDirection(Vector2 direction)
        {
            if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
                OnInput?.Invoke(this, new SwipeInputEventArgs(_startContext, _endContext, SwipeInputEventArgs.SwipeDirection.Up));
            if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
                OnInput?.Invoke(this, new SwipeInputEventArgs(_startContext, _endContext, SwipeInputEventArgs.SwipeDirection.Down));
            if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
                OnInput?.Invoke(this, new SwipeInputEventArgs(_startContext, _endContext, SwipeInputEventArgs.SwipeDirection.Left));
            if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
                OnInput?.Invoke(this, new SwipeInputEventArgs(_startContext, _endContext, SwipeInputEventArgs.SwipeDirection.Right));
        }

        private Vector2 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position * 10);
        }
    }
}
