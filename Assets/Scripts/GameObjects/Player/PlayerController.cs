using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private SpriteRenderer _render;
    [SerializeField] private Sprite _jumpFlySprite;
    [SerializeField] private Sprite _jumpStartSprite;
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _keyBoardSensitivity;
    [SerializeField] private float _arrowsSensitivity;
    [SerializeField] private float _gyroscopeSensitivity;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;
    #endregion

    #region METHODS
    private void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        InputCatcher();   
        ToogleCollider();
        CheckIntersectionWithScreenBot();
    }

    private void InputCatcher()
    {
        float moveDirection = GetMoveDirection();
        float jump = Input.GetAxis("Jump");

        if (moveDirection != 0) Move(moveDirection);
        if (jump != 0) Jump();
    }   

    private float GetMoveDirection()
    {
        return GameConfig.InputType switch
        {
            InputType.Keyboard => Input.GetAxis("Horizontal") * _keyBoardSensitivity,
            InputType.Arrows => ScreenArrows.inputForce * _arrowsSensitivity,
            InputType.Gyroscope => Input.acceleration.x * _gyroscopeSensitivity,
            _ => Input.GetAxis("Horizontal"),
        };
    }

    private void Move(float moveDirection)
    {
        _rigidbody.velocity = new Vector2(moveDirection, _rigidbody.velocity.y);

        float cameraWidth = CameraController.GetCameraSize().x;
        float positionX = _transform.position.x;
        float positionY = _transform.position.y;

        if (positionX > cameraWidth)
        {
            _transform.position = new Vector2(-cameraWidth, positionY);
        } 
        else if (positionX < -cameraWidth)
        {
            _transform.position = new Vector2(cameraWidth, positionY);
        }

        if (GameConfig.isGamePaused) return;

        if (moveDirection > 0 && _render.flipX)
        {
            _render.flipX = false;
        }
        else if (moveDirection < 0 && !_render.flipX)
        {
            _render.flipX = true;
        }
    }

    private void Jump()
    {
        _rigidbody.velocity = Vector2.up * _jumpVelocity;
        StartCoroutine(SetJumpAnimation());
    }

    IEnumerator SetJumpAnimation()
    {
        _render.sprite = _jumpStartSprite;
        yield return new WaitForSeconds(0.3f);
        _render.sprite = _jumpFlySprite;
    }

    private void ToogleCollider()
    {
        _collider.enabled = _rigidbody.velocity.y <= 0;
    }

    public void CheckIntersectionWithScreenBot()
    {
        float camY = CameraController.cameraTransform.position.y;
        float bottomReach = camY - CameraController.GetCameraSize().y;

        if (bottomReach > _transform.position.y + _transform.localScale.y) Death();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) Jump();
    }

    public Rigidbody2D GetRigidbody()
    {
        return _rigidbody;
    }

    private void Death()
    {
        GameConfig.IsGameOver = true;
        Destroy(gameObject);
    }
    #endregion
}
