using UnityEngine;

namespace Input
{
    /// <summary>
    /// Calculates aim direction by raycasting from camera through mouse position onto gameplay plane.
    /// The gameplay plane is perpendicular to X axis (YZ plane) to constrain movement to 2D.
    /// </summary>
    public class RaycastAimCalculator : IAimCalculator
    {
        private const float DirectionEpsilon = 0.001f;

        public Vector3 CalculateAimDirection(Vector2 screenPosition, Vector3 startPosition, Camera camera)
        {
            Plane gameplayPlane = new Plane(Vector3.right, Vector3.zero);
            Ray ray = camera.ScreenPointToRay(screenPosition);

            if (gameplayPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 direction = (hitPoint - startPosition).normalized;
                direction.x = 0;
                return direction.sqrMagnitude < DirectionEpsilon ? Vector3.zero : direction.normalized;
            }

            return Vector3.zero;
        }
    }
}

