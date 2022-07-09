using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Camera _uiCamera;

    private static CameraController instance;
    public static Camera mainCamera;
    public static Transform cameraTransform;
    private Transform _playerTransform;
    private float _currentY = 0;

    private void Start()
    {
        instance = this;
        mainCamera = Camera.main;
        cameraTransform = transform;
        _playerTransform = _player.transform;
    }
    private void Update()
    {
        if (!GameConfig.IsGameOver && _playerTransform.position.y > _currentY)
        {
            float x = cameraTransform.position.x;
            float y = _playerTransform.position.y;
            float z = cameraTransform.position.z;
            _currentY = y;
            cameraTransform.position = new Vector3(x, y, z);
            EventManager.SendUpdateLevelHeight(y);
            GameConfig.Score = Mathf.RoundToInt(y * 10);
        }
    }

    public static Vector2 GetCameraPositionZero(float cameraWidth)
    {
        mainCamera.orthographicSize = cameraWidth / mainCamera.aspect;
        instance._uiCamera.orthographicSize = cameraWidth / mainCamera.aspect;
        return new Vector2(-cameraWidth, -mainCamera.orthographicSize);
    }

    public static Vector2 GetCameraSize()
    {
        return new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);
    }
}
