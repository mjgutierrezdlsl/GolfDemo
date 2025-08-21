using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] BallController _ball;
    [SerializeField] HoleController _hole;
    [SerializeField] LineManager _lineManager;
    [SerializeField] Vector2 _spawnZone = Vector2.one;

    private void Start()
    {
        SetObjectPositions();
    }

    private void OnEnable()
    {
        _ball.OnHoleScored += SetObjectPositions;
        _lineManager.OnWallSpawned += OnWallSpawned;
    }

    private void OnDisable()
    {
        _ball.OnHoleScored -= SetObjectPositions;
        _lineManager.OnWallSpawned -= OnWallSpawned;
    }

    private void OnWallSpawned(LineWall wall)
    {
        SpawnIfNearWall(wall);
    }

    private void SpawnIfNearWall(LineWall wall)
    {
        if (!wall) return;
        var start = wall.LineRenderer.GetPosition(0);
        var end = wall.LineRenderer.GetPosition(1);
        var magnitude = (end - start).magnitude;
        print($"{_hole.transform.position}|{start}|{end}");
        for (int i = 0; i < wall.LineRenderer.positionCount; i++)
        {
            if (Vector2.Distance(_hole.transform.position, wall.LineRenderer.GetPosition(i)) < magnitude)
            {
                _hole.transform.position = GetRandomPosition();
            }
        }
    }

    private void SetObjectPositions()
    {
        _ball.SetPosition(GetRandomPosition());
        _hole.transform.position = GetRandomPosition();
        var wall = Physics2D.OverlapCircle(_hole.transform.position, 1f);
        if (wall != null)
        {
            SpawnIfNearWall(wall.GetComponent<LineWall>());
        }
    }

    private Vector2 GetRandomPosition()
    {
        var x = _spawnZone.x * 0.5f;
        var y = _spawnZone.y * 0.5f;
        return new Vector2(Random.Range(-x, x), Random.Range(-y, y));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _spawnZone);
    }
}
