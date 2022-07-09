using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Platform
{
    public PlatformType type;
    public GameObject prefab;
    public float spawnRate;
}

public class LevelGenerator : MonoBehaviour
{
    #region FIELDS
    [HideInInspector] public List<Platform> platforms = new();

    [SerializeField, Tooltip("Количество колонок в ряду")]
    private int _cols;

    [SerializeField, Tooltip("Ширина ячейки")]
    private float _cellWidth;

    [SerializeField, Tooltip("Высота ряда")]
    private float _rowHeight;

    [SerializeField, Tooltip("Дополнительный отступ по Y платформы")]
    private float _offsetY;

    [SerializeField, Tooltip("Высота по достижению которой генерируется новые платформы")]
    private float _levelHeight;

    [SerializeField, Tooltip("Минимальный шанс спавна платформы")]
    private int _minColSpawnRate = 20;

    [SerializeField, Tooltip("Максимальный шанс спавна платформы")]
    private int _maxColSpawnRate = 80;

    private readonly Dictionary<PlatformType, GameObject> _platformsDictionary = new();
    private readonly List<GameObject> _platformsPull = new();
    private readonly List<int> _spawnRateList = new();
    private readonly List<GameObject> _lastRow = new();
    private bool _isLastRowClear = false;
    private float _startHeight;
    private float _currentHeight = 0;
    private Vector2 _firstPoint;
    #endregion

    #region METHODS
    private void Start()
    {
        CreatePlatformsDictionary();
        CalcColSpawnRates();
        Init();
        InitPlayer();
    }

    private void OnEnable()
    {
        EventManager.OnUpdateLevelHeight.AddListener(UpdateLevelHeight);
    }

    private void Update()
    {
        PlatformSpawner();
    }

    private void CreatePlatformsDictionary()
    {
        foreach (Platform platform in platforms)
        {
            _platformsDictionary.Add(platform.type, platform.prefab);
        }
    }

    private void CalcColSpawnRates()
    {
        int spawnRateStep = Mathf.RoundToInt((_maxColSpawnRate - _minColSpawnRate) / _cols);
        for (int i = 0; i < _cols; i++)
        {
            int spawnRate = i != _cols - 1 ? _minColSpawnRate + spawnRateStep * i : _maxColSpawnRate;
            _spawnRateList.Add(spawnRate);
        }
    }

    private void Init()
    {
        _startHeight = _levelHeight;
        _firstPoint = new(_cellWidth / 2, _rowHeight / 2);
        float fieldHalfWidth = _cellWidth * _cols / 2;
        transform.position = CameraController.GetCameraPositionZero(fieldHalfWidth);

        while (_currentHeight < _levelHeight)
        {
            CreateRow(_currentHeight == 0);
        }
    }

    private void InitPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector2(_platformsPull[0].transform.position.x, _platformsPull[0].transform.position.y + 0.1f);
        GameConfig.StartGame();
    }

    private void PlatformSpawner()
    {
        if (_currentHeight < _levelHeight) CreateRow();
    }

    private void UpdateLevelHeight(float y)
    {
        _levelHeight = _startHeight + y;
    }
    
    private void CreateRow(bool isInit = false)
    {
        _lastRow.Clear();
        int createdCount = 0;

        for (int i = 0; i < _cols; i++)
        {
            GameObject prefab = isInit ? GetPlatformPrefab(PlatformType.Static) : GetPlatformPrefab(i, createdCount);

            if (prefab != null)
            {
                float x =_firstPoint.x + _cellWidth * i;
                float y = _firstPoint.y + _currentHeight + Random.Range(-_offsetY, _offsetY);
                CreatePlatform(new Vector2(x, y), prefab);
                _lastRow.Add(prefab);
                createdCount++;
            }
        }

        _currentHeight += _rowHeight;
        _isLastRowClear = _lastRow.FindAll(el => el.CompareTag("Platform")).Count == 0;
    }

    private GameObject CreatePlatform(Vector2 position, GameObject prefab)
    {
        GameObject platform = Instantiate(prefab, transform);
        platform.transform.localPosition = position;
        _platformsPull.Add(platform);
        return platform;
    }

    private GameObject GetPlatformPrefab(PlatformType type)
    {
        return _platformsDictionary[type];
    }

    private GameObject GetPlatformPrefab(int col, int createdCount)
    {
        GameObject prefab = null;
        _spawnRateList.Reverse();
        int rate = _isLastRowClear ? _spawnRateList[col - createdCount] * 2 : _spawnRateList[col - createdCount];
        bool isSpawn = Random.Range(0, 100) < rate;

        if (isSpawn)
        {
            if (_isLastRowClear)
            {
                _isLastRowClear = false;
                return _platformsDictionary[PlatformType.Static];
            }

            float totalTypesSpawnRate = 0;
            List<float[]> typeRates = new();

            foreach (Platform platform in platforms)
            {
                typeRates.Add(new float[2] { totalTypesSpawnRate, totalTypesSpawnRate + platform.spawnRate });
                totalTypesSpawnRate += platform.spawnRate;
            }

            float definingNum = Random.Range(0, totalTypesSpawnRate + 1);

            foreach (float[] rates in typeRates)
            {
                if (definingNum >= rates[0] && definingNum <= rates[1])
                {
                    prefab = platforms[typeRates.IndexOf(rates)].prefab;
                    break;
                }
            }
        }

        return prefab;
    }
    #endregion
}