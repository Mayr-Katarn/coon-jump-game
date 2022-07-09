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
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _jumpVelocity;
    //[HideInInspector] public bool isGoinDown;

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
        Debug.Log(SystemInfo.deviceType);
    }

    private void Update()
    {
        InputCatcher();   
        ToogleCollider();
        CheckIntersectionWithScreenBot();
    }

    private void InputCatcher()
    {
        float moveDirection = Input.acceleration.x * 4;
        //float moveDirection = SystemInfo.deviceType == DeviceType.Handheld ? Input.acceleration.x * 4 : Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");

        if (moveDirection != 0) Move(moveDirection);
        if (jump != 0) Jump();

        SetBodyOrientation(moveDirection);
    }

    private void Move(float moveDirection)
    {
        _transform.Translate(moveDirection * Time.deltaTime * new Vector2(_maxMoveSpeed, 0));

        float cameraWidth = CameraController.GetCameraSize().x;
        float positionY = _transform.position.y;
        float positionX = _transform.position.x;

        if (positionX > cameraWidth)
        {
            _transform.position = new Vector2(-cameraWidth, positionY);
        } 
        else if (positionX < -cameraWidth)
        {
            _transform.position = new Vector2(cameraWidth, positionY);
        }
    }

    private void Jump()
    {
        Vector2 force = Vector2.up * _jumpVelocity;
        _rigidbody.velocity = force;
        StartCoroutine(SetJumpAnimation());
    }

    IEnumerator SetJumpAnimation()
    {
        _render.sprite = _jumpStartSprite;
        yield return new WaitForSeconds(0.3f);
        _render.sprite = _jumpFlySprite;
    }

    private void SetBodyOrientation(float direction)
    {
        float x = _transform.localScale.x;
        float y = _transform.localScale.y;

        if (direction > 0 && x < 0)
        {
            _transform.localScale = new Vector2(Mathf.Abs(x), y);
        }
        else if (direction < 0 && x > 0)
        {
            _transform.localScale = new Vector2(x * -1, y);
        }
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
