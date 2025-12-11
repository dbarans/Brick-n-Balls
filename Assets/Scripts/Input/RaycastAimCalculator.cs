using UnityEngine;

namespace Input
{
    public class RaycastAimCalculator : IAimCalculator
    {
        public Vector3 CalculateAimDirection(Vector2 screenPosition, Vector3 startPosition, Camera camera)
        {
            Plane gameplayPlane = new Plane(Vector3.right, Vector3.zero);
            Ray ray = camera.ScreenPointToRay(screenPosition);

            if (gameplayPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 direction = (hitPoint - startPosition).normalized;
                direction.x = 0;
                return direction.sqrMagnitude < 0.001f ? Vector3.zero : direction.normalized;
            }

            return Vector3.zero;
        }
    }
}

