using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Input;
using Data;
using Game;

/// <summary>
/// Coordinates player input and updates ball aiming direction before firing.
/// </summary>
public class GameInputManager : MonoBehaviour
{
    private const float DirectionEpsilon = 0.001f;
    private const float DebugLineLength = 5f;

    private IInputProvider _inputProvider;
    private IEntityDataAccessor _entityDataAccessor;
    private IAimCalculator _aimCalculator;
    private Camera _mainCamera;
    private IAimLineRenderer _aimLineRenderer;

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

        var aimLineRendererComponent = GetComponent<AimLineRenderer>();
        _aimLineRenderer = aimLineRendererComponent;
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
        // Don't process input until game is started
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null && world.IsCreated)
        {
            var entityManager = world.EntityManager;
            var gameStateQuery = entityManager.CreateEntityQuery(typeof(GameState));
            if (gameStateQuery.HasSingleton<GameState>())
            {
                var gameState = gameStateQuery.GetSingleton<GameState>();
                if (!gameState.IsGameStarted) return;
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

        if (!_entityDataAccessor.HasBallEntity()) return;

        
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
            
           
            if (_mainCamera == null)
            {
                var allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
                if (allCameras.Length > 0)
                {
                    _mainCamera = allCameras[0];
                }
            }
        }

        if (_mainCamera == null || !_mainCamera.enabled) return;

        Vector2 mouseScreenPos = _inputProvider.GetAimInput();
        var ballData = _entityDataAccessor.GetBallComponent();
        var startPos = _entityDataAccessor.GetBallPosition();

        if (ballData.IsFired) return;

        Vector3 direction = _aimCalculator.CalculateAimDirection(mouseScreenPos, (Vector3)startPos, _mainCamera);

        if (direction.sqrMagnitude < DirectionEpsilon) return;

        ballData.InitialDirection = direction;

        if (_aimLineRenderer != null)
        {
            _aimLineRenderer.UpdateAimLine((Vector3)startPos, direction);
        }

        if (_inputProvider.GetFireInput())
        {
            ballData.IsFired = true;
            
            if (_aimLineRenderer != null)
            {
                _aimLineRenderer.HideLine();
            }
        }

        _entityDataAccessor.SetBallComponent(ballData);
    }
}
