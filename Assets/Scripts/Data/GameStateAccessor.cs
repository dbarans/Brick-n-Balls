using Unity.Entities;

namespace Data
{
    public class GameStateAccessor : IGameStateAccessor
    {
        private EntityManager _entityManager;
        private EntityQuery _gameStateQuery;

        public GameStateAccessor(EntityManager entityManager)
        {
            _entityManager = entityManager;
            _gameStateQuery = _entityManager.CreateEntityQuery(typeof(GameState));
        }

        public bool TryGetGameState(out GameState gameState)
        {
            if (_gameStateQuery.IsEmpty)
            {
                gameState = default;
                return false;
            }

            var entity = _gameStateQuery.GetSingletonEntity();
            gameState = _entityManager.GetComponentData<GameState>(entity);
            return true;
        }

        public void SetGameState(GameState gameState)
        {
            if (_gameStateQuery.IsEmpty) return;

            var entity = _gameStateQuery.GetSingletonEntity();
            _entityManager.SetComponentData(entity, gameState);
        }

        public bool HasGameState()
        {
            return !_gameStateQuery.IsEmpty;
        }
    }
}

