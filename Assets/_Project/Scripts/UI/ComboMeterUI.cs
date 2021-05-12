using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
	public class ComboMeterUI : MonoBehaviour
	{
		[SerializeField] private Image      fillImage;
		[SerializeField] private GameObject highlight;

		private void Awake()
		{
			fillImage.fillAmount = 0f;
			highlight.SetActive(false);
			UpdateUI(0f);
		}
		
		public void UpdateUI(float percent)
		{
			fillImage.fillAmount = percent;
			highlight.SetActive(percent >= 1f);
		}
	}
}