using UnityEngine;
using UnityEngine.InputSystem;

public class Movment : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    private PlayerMovment _playerMovment;
    [SerializeField] private float _sentive = 5;
    [SerializeField] private Transform _camera;
    private Rigidbody _rb;
    private float rotation;
    [SerializeField]private Transform _car;
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minFOV = 20f;
    [SerializeField] private float _maxFOV = 60f;
    [SerializeField] private Camera _camComponent;
    private float _targetFOV;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovment = new PlayerMovment();
        _targetFOV = _camComponent.fieldOfView;
        _playerMovment.Player.Jump.performed += context => OnJump();
        _playerMovment.Player.Sit.performed += context => SwitchToVehicleControls();
        _playerMovment.Vehicle.Exit.performed += context => SwitchToPlayerControls();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SwitchToPlayerControls();
    }

    private void OnEnable()
    {
        _playerMovment.Enable();
    }

    private void OnDisable()
    {
        _playerMovment.Disable();
    }
    
    public void SwitchToPlayerControls()
    {
        _playerMovment.Player.Enable();
        _playerMovment.Vehicle.Disable();
    }
    
    public void SwitchToVehicleControls()
    {
        _playerMovment.Player.Disable();
        _playerMovment.Vehicle.Enable();
    }
    
    void Update()
    {
        if (_playerMovment.Player.enabled)
        {
            ZoomCamera();
            Vector2 movement = _playerMovment.Player.Move.ReadValue<Vector2>();
            Move(movement);
            Vector2 mouse = _playerMovment.Player.Look.ReadValue<Vector2>();
            Look(mouse);
        }
        else if (_playerMovment.Vehicle.enabled)
        {
            Vector2 vehicleInput = _playerMovment.Vehicle.Drive.ReadValue<Vector2>();
            DriveVehicle(vehicleInput);
        }
    }

    private void Move(Vector2 directionMove)
    {
        Vector3 movement = new Vector3(directionMove.x, 0, directionMove.y);
        Vector3 MoveVector = transform.TransformDirection(movement);
        transform.position += MoveVector * _moveSpeed * Time.deltaTime;
    }

    private void Look(Vector2 directionLook)
    {
        rotation -= directionLook.y * _sentive;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        this.transform.Rotate(0f, directionLook.x * _sentive, 0f);
        _camera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
    }

    private void OnJump()
    {
        _rb.AddForce(Vector3.up * _moveSpeed, ForceMode.VelocityChange);  
    }
    
    private void DriveVehicle(Vector2 input)
    {
        Vector3 movement = new Vector3(input.x, 0, input.y);
        Vector3 MoveVector = _car.TransformDirection(movement);
        _car.position += new Vector3(movement.y * _moveSpeed * Time.deltaTime,0,0);
        _car.Rotate(0, MoveVector.x * _moveSpeed * Time.deltaTime, 0);
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
