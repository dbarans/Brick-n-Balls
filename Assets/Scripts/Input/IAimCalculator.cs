using UnityEngine;

namespace Input
{
    /// <summary>
    /// Calculates aim direction from screen position using raycasting.
    /// </summary>
    public interface IAimCalculator
    {
        Vector3 CalculateAimDirection(Vector2 screenPosition, Vector3 startPosition, Camera camera);
    }
}

