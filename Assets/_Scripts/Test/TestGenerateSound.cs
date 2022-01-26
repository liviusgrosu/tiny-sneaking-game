using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerateSound : MonoBehaviour
{
    [SerializeField] private GameSoundController _soundController;
    [SerializeField] private GameObject _soundSphere;
    [SerializeField] private float _interval;

    private void Start()
    {
        InvokeRepeating("TriggerSound", 0.0f, _interval);
    }

    private void TriggerSound()
    {
        _soundController.PlaySpecificSound("Wood Footstep", transform.position);
    }
}
