using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Movement _playerMovementComponent;
    private Vector2 _targetPosition;
    private Vector2 _directionToTarget;
    private bool _isMovingToTarget;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            UpdateTarget();
        WalkToLastClick();
    }
    private void UpdateTarget()
    {
        _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _directionToTarget = (_targetPosition - (Vector2)_playerMovementComponent.transform.position).normalized;
        _isMovingToTarget = true;
    }
    private void WalkToLastClick()
    {
        if (_isMovingToTarget)
        {
            if (!TargetReached())
            {
                _playerMovementComponent.Move(_directionToTarget);
                _directionToTarget = (_targetPosition - (Vector2)_playerMovementComponent.transform.position).normalized;
            }
            else
            {
                _isMovingToTarget = false;
                _playerMovementComponent.Move(Vector2.zero);
            }
        }
    }

    private bool TargetReached()
    {
        return Vector2.Distance(_targetPosition, _playerMovementComponent.transform.position) < 0.2f;
    }
}
