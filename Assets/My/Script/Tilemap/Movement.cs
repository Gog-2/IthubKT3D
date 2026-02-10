using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TilemapScripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationJump;
        [SerializeField] private string _animationWalk;
        private PlayerMovment _playerMovment;
        private void Awake()
        {
            _playerMovment = new PlayerMovment();
        }
        private void OnEnable()
        {
            _playerMovment.Enable();
        }
        private void OnDisable()
        {
            _playerMovment.Disable();
        }
        private void Start()
        {
            _playerMovment.Tilemap.Jump.performed += context => Jump();
        }
        private void FixedUpdate()
        {
            float mov = _playerMovment.Tilemap.Move.ReadValue<float>();
            Animations();
            Move(mov);
        }
        private void Animations()
        {
            if (_rigidbody.linearVelocity.x == 0 && _animator.GetBool(_animationWalk))
            {
                _animator.SetBool(_animationWalk, false);
            }

            if (Math.Abs(_rigidbody.linearVelocity.x) > 0 && !_animator.GetBool(_animationWalk))
            {
                _animator.SetBool(_animationWalk, true);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            _animator.SetBool(_animationJump, false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _animator.SetBool(_animationJump, true);
        }

        private void Move(float direction)
        {
            _rigidbody.linearVelocity = new Vector2(direction * _speed, _rigidbody.linearVelocity.y);
        }
        private void Jump()
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
        }
    }
}