using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] float _maxLength = 2f;
    [SerializeField] float _strikeForce = 10f;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] ParticleSystem _hitEffect;

    Camera _camera;
    Rigidbody2D _rigidbody;

    Vector2 _mouseStartPosition, _mouseEndPosition;

    public event Action OnHoleScored;
    public event Action<Vector2, Vector2> OnMouseReleased;

    private void Awake()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _mouseStartPosition = transform.position;
        _mouseEndPosition = transform.position;
    }

    private void Update()
    {
        var direction = _mouseStartPosition - _mouseEndPosition;
        direction = Vector2.ClampMagnitude(direction, _maxLength);

        // Prevents the player from spamming inputs
        if (_rigidbody.linearVelocity.magnitude > 0.3f)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            _mouseEndPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, (Vector2)transform.position + direction);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _rigidbody.AddForce(direction * _strikeForce, ForceMode2D.Impulse);
            _lineRenderer.gameObject.SetActive(false);
            OnMouseReleased?.Invoke(_mouseStartPosition, _mouseEndPosition);
        }
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        _trailRenderer.gameObject.SetActive(true);
    }

    public void ScoreHole()
    {
        var particles = Instantiate(_hitEffect, transform.position, Quaternion.identity);
        particles.Play();
        _trailRenderer.gameObject.SetActive(false);
        OnHoleScored?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.GetContact(0);
        var hitEffect = Instantiate(_hitEffect, contact.point, Quaternion.Euler(contact.normal));
        hitEffect.Play();
    }
}
