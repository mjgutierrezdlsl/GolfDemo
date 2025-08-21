using System;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    // [SerializeField] float _minLength = 1f;
    [SerializeField] LineWall _linePrefab;
    [SerializeField] BallController _ball;
    public readonly List<LineWall> LineWalls = new();
    public event Action<LineWall> OnWallSpawned;
    private void OnEnable()
    {
        _ball.OnMouseReleased += OnMouseReleased;
    }
    private void OnDisable()
    {
        _ball.OnMouseReleased -= OnMouseReleased;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearWalls();
        }
    }
    private void OnMouseReleased(Vector2 start, Vector2 end)
    {
        if (Vector2.Distance(start, end) < 1f) { return; }
        var wall = Instantiate(_linePrefab, transform.position, Quaternion.identity);
        wall.SetPoints(start, end);
        LineWalls.Add(wall);
        OnWallSpawned?.Invoke(wall);
    }
    public void ClearWalls()
    {
        for (int i = LineWalls.Count - 1; i >= 0; i--)
        {
            var wall = LineWalls[i];
            Destroy(wall.gameObject);
        }
        LineWalls.Clear();
    }
}
