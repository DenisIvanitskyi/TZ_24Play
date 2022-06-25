using UnityEngine;

namespace Assets.Game.UI
{
    public class StartToGameSplashScreen : MonoBehaviour, IStartToGameSplashScreenController
    {
        public const string PART_PointerName = "Pointer";
        private int _direction = 1;

        [SerializeField]
        public RectTransform _pointerTransform;

        [SerializeField]
        private Vector2 _startPoint = new Vector2(-150, -25);

        [SerializeField]
        private Vector2 _endPoint = new Vector2(-150, -25);

        [SerializeField]
        private bool _isRunningAnimationOfPointer;

        [SerializeField]
        private float _pointerSpeed = 65f;

        public bool IsRunningAnimationOfPointer
        {
            get => _isRunningAnimationOfPointer;
            set => _isRunningAnimationOfPointer = value;
        }

        public float PointerAnimtionSpeed
        {
            get => _pointerSpeed;
            set => _pointerSpeed = value;
        }

        public bool IsVisible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void Start()
        {
            IsRunningAnimationOfPointer = true;
        }

        public void FixedUpdate()
        {
            MovePointer();
        }

        private void MovePointer()
        {
            if (IsRunningAnimationOfPointer && _pointerTransform != null)
            {
                var newPositionX = _pointerTransform.localPosition.x + PointerAnimtionSpeed * Time.fixedDeltaTime * _direction;
                _pointerTransform.localPosition = new Vector2(newPositionX, _startPoint.y);

                if (newPositionX <= _startPoint.x)
                    _direction = 1;
                else if (newPositionX >= _endPoint.x)
                    _direction = -1;
            }
        }
    }
}
