using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : StaticPlatform
{
    #region FIELDS
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private MovingPlatformType _movingType;
    private float _distance;
    private float _speed;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _destination;
    private bool _isMovingForward = true;
    #endregion

    #region METHODS
    protected override void Start()
    {
        base.Start();
        _movingType = Random.Range(0, 2) == 0 ? MovingPlatformType.Horizontal : MovingPlatformType.Vertical;
        _distance = Random.Range(_minDistance, _maxDistance);
        _speed = Random.Range(_minSpeed, _maxSpeed);
        _startPosition = _transform.localPosition;
        _endPosition = _movingType == MovingPlatformType.Horizontal ?
            new Vector2(_startPosition.x + _distance, _startPosition.y) : new Vector2(_startPosition.x, _startPosition.y + _distance);

        _destination = _endPosition;
    }

    protected override void Update()
    {
        base.Update();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (Vector2.Distance(_transform.localPosition, _destination) == 0)
        {
            _isMovingForward = !_isMovingForward;
            _destination = _isMovingForward ? _endPosition : _startPosition;
        }

        _transform.localPosition = Vector2.MoveTowards(_transform.localPosition, _destination, _speed * Time.deltaTime);
    }
    #endregion
}
