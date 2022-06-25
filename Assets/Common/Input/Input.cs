using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Common.Input
{
    public class Input : MonoBehaviour
    {
        [NonSerialized]
        private InputControls _inputControls;

        [NonSerialized]
        private List<Action<InputAction.CallbackContext>> _inputs;

        private void Awake()
        {
            _inputControls = new InputControls();
            _inputs = new List<Action<InputAction.CallbackContext>>();
        }

        private void OnEnable() => _inputControls?.Enable();

        private void OnDisabled() => _inputControls?.Disable();

        private void Start()
        {
            foreach (var input in _inputs)
                SubscibeOnTap(input);
            _inputs.Clear();
        }

        public void SubscibeOnTap(Action<InputAction.CallbackContext> action)
        {
            if (_inputControls != null)
                _inputControls.Tap.Tap.canceled += action;
            else
                _inputs.Add(action);
        }

        public void UnsubscribeTap(Action<InputAction.CallbackContext> action)
        {
            _inputControls.Tap.Tap.canceled -= action;
        }
    }
}
