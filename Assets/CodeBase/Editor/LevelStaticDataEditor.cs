using CodeBase.Logic;
using CodeBase.Services.StaticData;
using CodeBase.SoundManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialSpawnPointCollectorTag = "SpawnPointCollectorTag";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData) target;

            if (GUILayout.Button("Collect"))
            {
                levelData.LevelKey = SceneManager.GetActiveScene().name;
                SoundManagerMarker soundManagerMarker = FindObjectOfType<SoundManagerMarker>();
                levelData.SoundManagerData = new SoundManagerData(soundManagerMarker.sounds, soundManagerMarker.clips,
                    soundManagerMarker.soundManagerType);
                GameObject spawnPointCollectorGO = GameObject.FindGameObjectWithTag(InitialSpawnPointCollectorTag);
                SpawnPointCollector spawnPointCollector = spawnPointCollectorGO.GetComponent<SpawnPointCollector>();
                levelData.SpawnPoints = spawnPointCollector.GetPositions();
            }

            EditorUtility.SetDirty(target);
        }
    }
}