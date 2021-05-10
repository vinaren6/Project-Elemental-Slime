using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.WaveSystem.Editor
{
	[CustomEditor(typeof(WaveSpawner))]
	[CanEditMultipleObjects]
	public class WaveSpawnerEditor : UnityEditor.Editor
	{
		private SerializedProperty _activeSpawns;
		private SerializedProperty _totalSpawns;

		private void OnEnable()
		{
			_totalSpawns  = serializedObject.FindProperty("totalSpawns");
			_activeSpawns = serializedObject.FindProperty("activeSpawns");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawPropertiesExcluding(serializedObject, "totalSpawns", "activeSpawns");
			if (Application.isPlaying) {
				EditorGUILayout.Space();
				EditorGUILayout.LabelField($"Total Spawns: {_totalSpawns.intValue}");
				EditorGUILayout.LabelField($"Active Spawns: {_activeSpawns.intValue}");
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}