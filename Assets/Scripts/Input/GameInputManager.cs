using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Input;
using Data;

/// <summary>
/// Coordinates player input and updates ball aiming direction before firing.
/// </summary>
public class GameInputManager : MonoBehaviour
{
    private IInputProvider _inputProvider;
    private IEntityDataAccessor _entityDataAccessor;
    private IAimCalculator _aimCalculator;
    private Camera _mainCamera;

    /// <summary>
    /// Initializes dependencies via dependency injection. Falls back to default implementations if null.
    /// </summary>
    public void Initialize(IInputProvider inputProvider, IEntityDataAccessor entityDataAccessor, IAimCalculator aimCalculator, Camera camera)
    {
        _inputProvider = inputProvider;
        _entityDataAccessor = entityDataAccessor;
        _aimCalculator = aimCalculator;
        _mainCamera = camera;
    }

    private void Awake()
    {
        if (_inputProvider == null)
        {
            _inputProvider = new GameControlsInputProvider();
        }

        if (_entityDataAccessor == null)
        {
            var world = World.DefaultGameObjectInjectionWorld;
            _entityDataAccessor = new EntityDataAccessor(world.EntityManager);
        }

        if (_aimCalculator == null)
        {
            _aimCalculator = new RaycastAimCalculator();
        }

        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        _inputProvider?.Enable();
    }

    private void OnDisable()
    {
        _inputProvider?.Disable();
    }

    private void Update()
    {
        HandleInputAndSync();
        _inputProvider?.ResetFireRequest();
    }

    private void HandleInputAndSync()
    {
        if (!_entityDataAccessor.HasBallEntity()) return;

        Vector2 mouseScreenPos = _inputProvider.GetAimInput();
        var ballData = _entityDataAccessor.GetBallComponent();
        var startPos = _entityDataAccessor.GetBallPosition();

        if (ballData.isFired) return;

        Vector3 direction = _aimCalculator.CalculateAimDirection(mouseScreenPos, (Vector3)startPos, _mainCamera);

        if (direction.sqrMagnitude < 0.001f) return;

        ballData.initialDirection = direction;

        if (_inputProvider.GetFireInput())
        {
            ballData.isFired = true;
        }

        _entityDataAccessor.SetBallComponent(ballData);
        Debug.DrawLine(startPos, (Vector3)startPos + direction * 5f, Color.green);
    }
}
