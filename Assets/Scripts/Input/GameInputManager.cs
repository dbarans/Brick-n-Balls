using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class GameInputManager : MonoBehaviour
{
    private IInputProvider _inputProvider;
    private IEntityDataAccessor _entityDataAccessor;
    private Camera _mainCamera;

    private void Awake()
    {
        _inputProvider = new GameControlsInputProvider();
        
        var world = World.DefaultGameObjectInjectionWorld;
        _entityDataAccessor = new EntityDataAccessor(world.EntityManager);
        
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputProvider.Enable();
    }

    private void OnDisable()
    {
        _inputProvider.Disable();
    }

    private void Update()
    {
        HandleInputAndSync();
        _inputProvider.ResetFireRequest();
    }

    private void HandleInputAndSync()
    {
        if (!_entityDataAccessor.HasBallEntity()) return;

        Vector2 mouseScreenPos = _inputProvider.GetAimInput();
        var ballData = _entityDataAccessor.GetBallComponent();
        var startPos = _entityDataAccessor.GetBallPosition();

        if (ballData.isFired) return;

        Plane gameplayPlane = new Plane(Vector3.right, Vector3.zero);
        Ray ray = _mainCamera.ScreenPointToRay(mouseScreenPos);

        if (gameplayPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = (hitPoint - (Vector3)startPos).normalized;

            direction.x = 0;

            if (direction.sqrMagnitude < 0.001f) return;

            direction = direction.normalized;
            ballData.initialDirection = direction;

            if (_inputProvider.GetFireInput())
            {
                ballData.isFired = true;
            }

            _entityDataAccessor.SetBallComponent(ballData);
            Debug.DrawLine(startPos, hitPoint, Color.green);
        }
    }
}
