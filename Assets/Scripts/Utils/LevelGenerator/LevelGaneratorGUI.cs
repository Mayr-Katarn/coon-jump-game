using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGaneratorGUI : Editor
{
    private LevelGenerator _levelGenerator;

    private void OnEnable()
    {
        _levelGenerator = (LevelGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (Platform platform in _levelGenerator.platforms.ToList())
        {
            GUILayout.BeginVertical("box");

            platform.type = (PlatformType)EditorGUILayout.EnumFlagsField("Platform type", platform.type);
            platform.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", platform.prefab, typeof(GameObject), false);
            platform.spawnRate = EditorGUILayout.FloatField("Spawn rate", platform.spawnRate);

            if (GUILayout.Button("Remove", GUILayout.MaxWidth(100))) _levelGenerator.platforms.Remove(platform);
            GUILayout.EndVertical();
            GUILayout.Space(6);
        }

        if (GUILayout.Button("Add new platform")) _levelGenerator.platforms.Add(new Platform());
        if (GUI.changed) SetObjectDirty(_levelGenerator.gameObject);
    }

    public static void SetObjectDirty(GameObject gameObject)
    {
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
}
