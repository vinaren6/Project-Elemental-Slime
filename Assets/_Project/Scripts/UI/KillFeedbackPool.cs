using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class KillFeedbackPool : MonoBehaviour
	{
		[SerializeField] private GameObject killTextPrefab;
		[SerializeField] private GameObject comboTextPrefab;
		[SerializeField] private int        killTextPoolSize = 10;
		[SerializeField] private int        comboTextPoolSize = 10;

		private Queue<GameObject> _killTextPool;
		private Queue<GameObject> _comboTextPool;

		private void Start()
		{
			_killTextPool = new Queue<GameObject>();
			for (int i = 0; i < killTextPoolSize; i++) {
				GameObject killText = Instantiate(killTextPrefab, transform);
				_killTextPool.Enqueue(killText);
			}
			
			_comboTextPool = new Queue<GameObject>();
			for (int i = 0; i < comboTextPoolSize; i++) {
				GameObject comboText = Instantiate(comboTextPrefab, transform);
				_comboTextPool.Enqueue(comboText);
			}
		}

		public void SpawnKillTextFromPool(Vector3 position)
		{
			GameObject killText = _killTextPool.Dequeue();
			killText.GetComponent<KillTextUI>().ShowKillText(position);
		}

		public void ReturnKillTextToPool(GameObject killText)
		{
			_killTextPool.Enqueue(killText);
		}
		
		public void SpawnComboTextFromPool(Vector3 position)
		{
			GameObject comboText = _comboTextPool.Dequeue();
			comboText.GetComponent<ComboTextUI>().ShowComboText(position);
		}

		public void ReturnComboTextToPool(GameObject comboText)
		{
			_comboTextPool.Enqueue(comboText);
		}
	}
}