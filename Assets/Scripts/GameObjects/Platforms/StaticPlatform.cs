using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StaticPlatform : MonoBehaviour
{
    #region FIELDS
    [HideInInspector] public BoxCollider2D _collider;
    [HideInInspector] public Transform _transform;
    #endregion

    #region METHODS
    protected virtual void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        CheckIntersectionWithScreenBot();
    }

    public GameObject Init(Vector2 position)
    {
        transform.localPosition = position;
        return gameObject;
    }

    public void CheckIntersectionWithScreenBot()
    {
        float camY = CameraController.cameraTransform.position.y;
        float bottomReach = camY - CameraController.GetCameraSize().y;

        if (bottomReach > transform.position.y) Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion
}
