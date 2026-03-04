using My.Script._3D;
using UnityEngine;

public class Movement3D : MonoBehaviour, CanBeActiveted
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _sentive = 5;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _jumpBufferTime = 0.15f;
    [SerializeField] private float _coyoteTime = 0.15f;
    private PlayerMovment _playerMovment;
    private Vector3 _velocity;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    [Header("Setup Components")]
    [SerializeField] private CharacterController _characterController;
    private float rotation;

    [SerializeField] private int _layer;
    public int Layer {
        get { return _layer; }
        set { _layer = value; }
    }

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minFOV = 20f;
    [SerializeField] private float _maxFOV = 60f;
    [SerializeField] private Camera _camComponent;
    private float _targetFOV;

    void Awake()
    {
        _playerMovment = new PlayerMovment();
        _targetFOV = _camComponent.fieldOfView;
        _playerMovment.Player.Jump.performed += context => OnJump();
        _playerMovment.Vehicle.Exit.performed += context => SwitchToPlayerControls();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SwitchToPlayerControls();
    }

    private void OnEnable() => _playerMovment.Enable();
    private void OnDisable() => _playerMovment.Disable();

    public void SwitchToPlayerControls()
    {
        _playerMovment.Player.Enable();
    }

    void Update()
    {
        ZoomCamera();

        Vector2 mouse = _playerMovment.Player.Look.ReadValue<Vector2>();
        Look(mouse);
        
        if (_jumpBufferCounter > 0f)
            _jumpBufferCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        GravityScaler();

        Vector2 movement = _playerMovment.Player.Move.ReadValue<Vector2>();
        Move(movement);

        HandleJump();
        Gravity();
    }

    private void GravityScaler()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        if (_characterController.isGrounded)
            _coyoteTimeCounter = _coyoteTime;
        else
            _coyoteTimeCounter -= Time.fixedDeltaTime;
    }

    private void Gravity()
    {
        _velocity.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);
    }

    private void Move(Vector2 directionMove)
    {
        Vector3 movement = new Vector3(directionMove.x, 0f, directionMove.y);
        Vector3 moveVector = transform.TransformDirection(movement);
        
        _characterController.Move(moveVector * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Look(Vector2 directionLook)
    {
        rotation -= directionLook.y * _sentive * Time.deltaTime;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        transform.Rotate(0f, directionLook.x * _sentive * Time.deltaTime, 0f);
        _camera.localRotation = Quaternion.Euler(rotation, 0f, 0f);
    }

    private void OnJump()
    {
        _jumpBufferCounter = _jumpBufferTime;
    }

    private void HandleJump()
    {
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _velocity.y = Mathf.Sqrt(_jumpPower * -2f * _gravity);
            _jumpBufferCounter = 0f;
            _coyoteTimeCounter = 0f;
        }
    }

    private void ZoomCamera()
    {
        float scrollInput = _playerMovment.Player.Zoom.ReadValue<float>();

        if (scrollInput != 0)
        {
            _targetFOV -= Mathf.Sign(scrollInput) * _zoomSpeed;
            _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);
        }

        _camComponent.fieldOfView = Mathf.Lerp(_camComponent.fieldOfView, _targetFOV, Time.deltaTime * 10f);
    }
}