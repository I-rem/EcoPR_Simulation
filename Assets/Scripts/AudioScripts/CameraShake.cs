using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin Camera;
    public float intensity;
    public float time;
    private float timer;
    private void Awake()
    {
        Camera = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        Camera.m_AmplitudeGain = intensity;
        timer = time;
    }

    private void Update()
    {
        if(timer > 0 )
        {
            StartCoroutine(StopShake(timer));
            timer = 0;
        }
    }

    IEnumerator StopShake(float a)
    {
        yield return new WaitForSeconds(a);
        Camera.m_AmplitudeGain = 0f;
    }
}
