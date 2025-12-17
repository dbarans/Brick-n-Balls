using Unity.Entities;

/// <summary>
/// Detects game over condition when all balls are depleted and triggers the game over event.
/// This system runs after ball destruction and spawning to ensure accurate state tracking.
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(BallDestructionSystem))]
[UpdateAfter(typeof(BallSpawningSystem))]
public partial class GameOverDetectionSystem : SystemBase
{
    private int _lastTrackedRemainingBalls = -1;
    private int _lastTrackedBallCount = -1;
    private bool _gameOverTriggered = false;

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingleton<GameState>(out var gameState)) return;

        // Reset game over flag when new game started
        if (gameState.RemainingBalls > _lastTrackedRemainingBalls && _lastTrackedRemainingBalls >= 0)
        {
            _gameOverTriggered = false;
        }

        if (gameState.RemainingBalls != _lastTrackedRemainingBalls)
        {
            _lastTrackedRemainingBalls = gameState.RemainingBalls;
        }

        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallComponent>().Build();
        int ballCount = ballQuery.CalculateEntityCount();
        
        if (ballCount != _lastTrackedBallCount)
        {
            _lastTrackedBallCount = ballCount;
        }

        // Trigger game over when all balls are depleted
        if (gameState.RemainingBalls == 0 && ballCount == 0)
        {
            TriggerGameOver();
        }
    }
    
    private void TriggerGameOver()
    {
        if (_gameOverTriggered) return;
        _gameOverTriggered = true;
        
        int finalScore = 0;
        if (SystemAPI.HasSingleton<GameScoreData>())
        {
            var scoreData = SystemAPI.GetSingleton<GameScoreData>();
            finalScore = scoreData.CurrentScore;
        }
        
        // Notify UI layer about game over
        GameEventBridge.OnGameOver?.Invoke(finalScore);
    }

    protected override void OnCreate()
    {
        _lastTrackedRemainingBalls = -1;
        _lastTrackedBallCount = -1;
        _gameOverTriggered = false;
    }
    
    protected override void OnStartRunning()
    {
        // Reset state when system starts
        _gameOverTriggered = false;
        _lastTrackedRemainingBalls = -1;
        _lastTrackedBallCount = -1;
    }
}

