using TMPro;
using UnityEngine;

namespace _Project.Scripts.WaveSystem
{
	public class DisplayCurrentWave : MonoBehaviour
	{
		[SerializeField] private TMP_Text text, staticText;

		[SerializeField] private float fadeInTime = 1, activeTime = 2, fadeOutTime = 3;

		private float _t;

		private void Start()
		{
			WaveController.Instance.onWaveStart.AddListener(OnNewRound);
			text.color   = new Color(text.color.a, text.color.b, text.color.g, 0);
			text.enabled = false;
			enabled      = false;
		}

		private void FixedUpdate()
		{
			if (_t < fadeInTime)
				text.color = new Color(text.color.a, text.color.b, text.color.g, _t / fadeInTime);
			else if (_t < fadeInTime + activeTime)
				text.color = new Color(text.color.a, text.color.b, text.color.g, 1);
			else if (_t < fadeInTime + activeTime + fadeOutTime)
				text.color = new Color(
					text.color.a, text.color.b, text.color.g,
					1 - (_t - fadeInTime - activeTime) / (fadeInTime + activeTime + fadeOutTime));
			else {
				text.enabled = false;
				enabled      = false;
			}

			_t += Time.fixedDeltaTime;
		}

		private void OnNewRound()
		{
			_t           = 0;
			enabled      = true;
			text.enabled = true;
			text.text    = staticText.text = ToRoman(WaveController.Instance.wave);
			text.color   = new Color(text.color.a, text.color.b, text.color.g, 0);
		}

		private static string ToRoman(int number)
		{
			if (number < 0 || number > 3999) return number.ToString();
			if (number < 1) return string.Empty;
			if (number >= 1000) return "M" + ToRoman(number - 1000);
			if (number >= 900) return "CM" + ToRoman(number - 900);
			if (number >= 500) return "D"  + ToRoman(number - 500);
			if (number >= 400) return "CD" + ToRoman(number - 400);
			if (number >= 100) return "C"  + ToRoman(number - 100);
			if (number >= 90) return "XC"  + ToRoman(number - 90);
			if (number >= 50) return "L"   + ToRoman(number - 50);
			if (number >= 40) return "XL"  + ToRoman(number - 40);
			if (number >= 10) return "X"   + ToRoman(number - 10);
			if (number >= 9) return "IX"   + ToRoman(number - 9);
			if (number >= 5) return "V"    + ToRoman(number - 5);
			if (number == 4) return "IV"   + ToRoman(number - 4);
			return "I" + ToRoman(number - 1);
		}
	}
}