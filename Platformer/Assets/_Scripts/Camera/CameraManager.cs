using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    private static CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        TryGetComponent(out impulseSource);
    }

    private void Start()
    {
        
    }

    public static void CameraShake(float strength)
    {
        if (!impulseSource) return;

        impulseSource.GenerateImpulse(strength);
    }
}
