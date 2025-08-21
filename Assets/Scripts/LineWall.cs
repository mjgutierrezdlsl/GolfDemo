using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class LineWall : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public LineRenderer LineRenderer => _lineRenderer;
    private EdgeCollider2D _edgeCollider;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }
    public void SetPoints(params Vector2[] points)
    {
        _lineRenderer.positionCount = points.Length;
        _edgeCollider.points = points;
        for (int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            _lineRenderer.SetPosition(i, point);
        }
    }
}
