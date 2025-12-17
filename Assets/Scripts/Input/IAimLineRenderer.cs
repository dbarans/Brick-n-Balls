using UnityEngine;

namespace Input
{
    /// <summary>
    /// Abstraction for rendering aiming line visualization.
    /// </summary>
    public interface IAimLineRenderer
    {
        void UpdateAimLine(Vector3 startPosition, Vector3 direction);
        void HideLine();
    }
}

