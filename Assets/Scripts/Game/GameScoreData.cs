using Unity.Entities;

namespace Game
{
    /// <summary>
    /// Singleton component storing game score data.
    /// PreviousFrameScore is used to detect score changes between frames.
    /// </summary>
    public struct GameScoreData : IComponentData
{
    public int CurrentScore;
    public int PreviousFrameScore; 
}
}