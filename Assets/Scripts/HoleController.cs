using System;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    float _timeElapsed = 0f;
    float _stayTime = 1.0f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<BallController>(out var ball))
        {
            return;
        }

        var ballMagnitude = ball.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        if (ballMagnitude < 0.1f)
        {
            if (_timeElapsed < _stayTime)
            {
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                _timeElapsed = 0f;
                ball.ScoreHole();
            }
        }
    }
}
