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
        var colliders = Physics2D.OverlapCircleAll(_hole.transform.position, 1f);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<LineWall>(out var w))
                {
                    _hole.transform.position = GetRandomPosition();
                }
            }
        }
        // SpawnIfNearWall(wall);
    }

    private void SetObjectPositions()
    {
        _ball.SetPosition(GetRandomPosition());
        _hole.transform.position = GetRandomPosition();
        var colliders = Physics2D.OverlapCircleAll(_hole.transform.position, 1f);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<LineWall>(out var w))
                {
                    _hole.transform.position = GetRandomPosition();
                }
            }
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
