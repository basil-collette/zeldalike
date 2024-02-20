using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : SingletonGameObject<PlayerController>
    {
        #region Attributes: DIRECTION & GRAVITY

        [SerializeField] private float _speed;
        private CharacterController _characterController;
        public Vector2 _moveInput;
        public Vector2 _direction;

        #endregion
        #region Attributes: DASH

        [SerializeField] private DashAttributes _dashAttributes = DashAttributes.Default;
        private bool _isDashing;
        public event Action<int, int> OnDashAmmunitionChange;

        #endregion

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            InitInputEvents();
        }

        private void InitInputEvents()
        {
            PlayerInput playerInput = FindAnyObjectByType<PlayerInput>();
            playerInput.actions["Move"].performed += Move;
            playerInput.actions["Dash"].performed += Dash;
        }

        private void Update()
        {
            ApplyMovement();
        }

        #region MOVEMENT METHODS

        private void ApplyMovement()
        {
            float targetSpeed = _speed;

            if (_isDashing)
            {
                targetSpeed = _dashAttributes.Speed;
                _direction.y = 0;
            }

            _characterController.Move(_direction * targetSpeed * Time.deltaTime);
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (_isDashing)
                return;

            _moveInput = context.ReadValue<Vector2>();
            _direction = new Vector2(_moveInput.x, _moveInput.y);
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (!context.started || !CanDash())
                return;

            _isDashing = true;
            StartCoroutine(DashCo());
        }

        private IEnumerator DashCo()
        {
            yield return new WaitForSeconds(_dashAttributes.Duration);
            _isDashing = false;
        }

        private bool CanDash() => !_isDashing;

        #endregion
    }

    #region STRUCTS

    [Serializable]
    public struct DashAttributes
    {
        [Range(0, 100)] public float Speed;
        [Range(0, 10)] public float Duration;

        public static readonly DashAttributes Default = new DashAttributes(20, 1);

        public DashAttributes(float speed, float duration)
        {
            Speed = speed;
            Duration = duration;
        }
    }

    #endregion

}


