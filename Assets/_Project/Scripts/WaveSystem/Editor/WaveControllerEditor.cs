﻿using System.Linq;
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
			if (GUILayout.Button("Begin Next Wave"))
				(target as WaveController)?.StartNextWave();

			//show active vs inactive spawners
			WaveController wc;
			if ((wc = target as WaveController) != null && wc.spawners != null && wc.spawners.Length > 0) {
				EditorGUILayout.Space();
				var wss            = wc.spawners;
				int activeSpawners = wss.Count(spawner => spawner.isActiveAndEnabled);
				EditorGUILayout.LabelField($"Active Spawners: {activeSpawners} / {wss.Length}");
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