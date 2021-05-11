using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.WaveSystem.Editor
{
	[CustomEditor(typeof(WaveController))]
	[CanEditMultipleObjects]
	public class WaveControllerEditor : UnityEditor.Editor
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

			//button
			EditorGUILayout.Space();
			if (GUILayout.Button("Begin Next Wave") && Application.isPlaying)
				(target as WaveController)?.StartNextWave();


			WaveController wc;
			if ((wc = target as WaveController) != null) {
				if (wc.debugString != "") {
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(wc.debugString);
				}

				//show active vs inactive spawners
				if (wc.spawners != null && wc.spawners.Length > 0) {
					EditorGUILayout.Space();
					var wss            = wc.spawners;
					int activeSpawners = wss.Count(spawner => spawner.isActiveAndEnabled);
					EditorGUILayout.LabelField($"Active Spawners: {activeSpawners} / {wss.Length}");
				}
			}

			//show total and active enemies
			if (Application.isPlaying) {
				EditorGUILayout.Space();
				EditorGUILayout.LabelField($"Total Spawns: {_totalSpawns.intValue}");
				EditorGUILayout.LabelField($"Active Spawns: {_activeSpawns.intValue}");
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}