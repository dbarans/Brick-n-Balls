using UnityEngine;
using System.Collections.Generic;

namespace Input
{
    /// <summary>
    /// Renders a dashed aiming line that fades out with distance.
    /// Used to visualize ball aiming direction before firing.
    /// </summary>
    public class AimLineRenderer : MonoBehaviour, IAimLineRenderer
    {
        [Header("Line Settings")]
        [SerializeField] private float lineLength = 5f;
        [SerializeField] private float dashLength = 0.2f;
        [SerializeField] private float gapLength = 0.1f;
        [SerializeField] private float startAlpha = 1f;
        [SerializeField] private float endAlpha = 0f;
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] private float lineWidth = 0.05f;

        private List<LineRenderer> _dashRenderers = new List<LineRenderer>();
        private Vector3 _startPosition;
        private Vector3 _direction;
        private bool _isVisible;

        private void Awake()
        {
            CreateDashRenderers();
        }

        private void CreateDashRenderers()
        {
            float totalDashLength = dashLength + gapLength;
            int maxDashes = Mathf.Max(10, Mathf.CeilToInt(lineLength / totalDashLength) + 5);
            
            for (int i = 0; i < maxDashes; i++)
            {
                var dashObj = new GameObject($"Dash_{i}");
                dashObj.transform.SetParent(transform);
                dashObj.transform.localPosition = Vector3.zero;
                
                var lr = dashObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = true;
                lr.material = CreateLineMaterial();
                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;
                lr.positionCount = 2;
                lr.enabled = false;
                lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lr.receiveShadows = false;
                
                _dashRenderers.Add(lr);
            }
        }

        private Material CreateLineMaterial()
        {
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = lineColor;
            return mat;
        }

        /// <summary>
        /// Updates the aiming line to show direction from start position.
        /// </summary>
        public void UpdateAimLine(Vector3 startPosition, Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f)
            {
                HideLine();
                return;
            }

            _startPosition = startPosition;
            _direction = direction.normalized;
            _isVisible = true;
            DrawDashedLine();
        }

        /// <summary>
        /// Hides the aiming line.
        /// </summary>
        public void HideLine()
        {
            _isVisible = false;
            foreach (var lr in _dashRenderers)
            {
                if (lr != null)
                {
                    lr.enabled = false;
                }
            }
        }

        private void DrawDashedLine()
        {
            if (!_isVisible) return;

            float totalDashLength = dashLength + gapLength;
            float currentDistance = 0f;
            int dashIndex = 0;

            while (currentDistance < lineLength && dashIndex < _dashRenderers.Count)
            {
                var lr = _dashRenderers[dashIndex];
                if (lr == null) break;

                var dashStart = _startPosition + _direction * currentDistance;
                currentDistance += dashLength;
                if (currentDistance > lineLength)
                {
                    currentDistance = lineLength;
                }
                var dashEnd = _startPosition + _direction * currentDistance;

                float dashStartAlpha = Mathf.Lerp(startAlpha, endAlpha, (currentDistance - dashLength) / lineLength);
                float dashEndAlpha = Mathf.Lerp(startAlpha, endAlpha, currentDistance / lineLength);

                lr.SetPosition(0, dashStart);
                lr.SetPosition(1, dashEnd);

                var startColor = lineColor;
                startColor.a = dashStartAlpha;
                var endColor = lineColor;
                endColor.a = dashEndAlpha;

                lr.startColor = startColor;
                lr.endColor = endColor;
                lr.enabled = true;

                currentDistance += gapLength;
                dashIndex++;
            }

            for (int i = dashIndex; i < _dashRenderers.Count; i++)
            {
                if (_dashRenderers[i] != null)
                {
                    _dashRenderers[i].enabled = false;
                }
            }
        }
    }
}
