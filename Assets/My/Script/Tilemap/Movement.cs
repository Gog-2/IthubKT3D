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
        
    }
}
