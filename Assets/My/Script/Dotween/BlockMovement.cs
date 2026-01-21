using System;
using UnityEngine;
using DG.Tweening;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerMovment _playerMovment;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float jumpDistance = 2f;
    [SerializeField] private float jumpHeight = 1f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovment = new PlayerMovment();
        _playerMovment.DoTween.Forward.performed += context => Forward();
        _playerMovment.DoTween.Down.performed += context => Backward();
        _playerMovment.DoTween.Left.performed += context => Left();
        _playerMovment.DoTween.Right.performed += context => Right();
        _playerMovment.DoTween.Jump.performed += context => Jump();
    }

    private void OnEnable()
    {
        _playerMovment.Enable();
    }

    private void OnDisable()
    {
        _playerMovment.Disable();
    }

    private void Forward()
    {
        Vector3 targetPos = _rb.position + transform.forward * 2f;
        _rb.DOMove(targetPos, 0.5f).SetEase(Ease.OutQuad);
    }

    private void Backward()
    {
        Vector3 targetPos = _rb.position + -transform.forward * 2f;
        _rb.DOMove(targetPos, 0.5f).SetEase(Ease.OutQuad);
    }
    private void Left() => transform.DORotate(new Vector3(0, -90, 0), _rotateSpeed).SetRelative();
    private void Right() => transform.DORotate(new Vector3(0, 90, 0), _rotateSpeed).SetRelative();

    private void Jump()
    {
        Vector3 targetPos = _rb.position + transform.forward * jumpDistance;
        
        _rb.DOJump(targetPos, jumpHeight, 1, 0.5f).SetEase(Ease.Linear);
    }
}
