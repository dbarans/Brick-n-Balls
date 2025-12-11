using Unity.Entities;

namespace Data
{
    /// <summary>
    /// Provides access to game state data (remaining balls, spawn position, etc.).
    /// </summary>
    public interface IGameStateAccessor
    {
        bool TryGetGameState(out GameState gameState);
        void SetGameState(GameState gameState);
        bool HasGameState();
    }
}

