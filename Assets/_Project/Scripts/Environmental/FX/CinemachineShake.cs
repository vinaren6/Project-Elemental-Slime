using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineShake : MonoBehaviour
{
	public float defaultShakeTime = .1f;
	
	private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
	private CinemachineVirtualCamera           _cinemachineVirtualCamera;

	private float _shakeTimer;
	private float _shakeTimerTotal;
	private float _startIntensity;

	private void Awake()
	{
		_cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
		_cinemachineBasicMultiChannelPerlin =
			_cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	private void Update()
	{
		if (_shakeTimer <= 0) return;
		_shakeTimer -= Time.deltaTime;
		_cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(
			_startIntensity, 0f, 1 - _shakeTimer / _shakeTimerTotal);
	}

	public void ShakeCamera(float intensity) => ShakeCamera(intensity, defaultShakeTime);
	public void ShakeCamera(float intensity, float timer)
	{
		_cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
		_shakeTimer                                         = timer;
		_shakeTimerTotal                                    = timer;
		_startIntensity                                     = intensity;
	}
}