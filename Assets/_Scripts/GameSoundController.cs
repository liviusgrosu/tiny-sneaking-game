using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour
{
    [SerializeField] private GameObject _soundInstance;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _maxDistToPlayer;
    private GameManager _gameManager;    

    [Serializable]
    public struct Clips
    {
        public string name;
        public AudioClip audioClip;
    }

    [SerializeField] private Clips[] _clips;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public void PlaySound(string name, Vector3 position)
    {
        AudioClip clipToUse;
        foreach(Clips currentClip in _clips)
        {
            if (currentClip.name == name)
            {
                // Get the clip to use
                clipToUse = currentClip.audioClip;
                // Create the sound sphere and set the scale
                GameObject currentSoundInstance = Instantiate(_soundInstance, position, Quaternion.identity);
                // Setup variables for volume calculation
                float distToPlayer = Vector3.Distance(position, _gameManager.PlayerInstance.position);
                float volumeFactor = 1.0f - (distToPlayer / _maxDistToPlayer);
                Vector3 dirToPlayer = _gameManager.PlayerInstance.position - position;

                // Get all obstacles between sound source and player
                RaycastHit[] hits;
                hits = Physics.RaycastAll(position, dirToPlayer, dirToPlayer.magnitude, _obstacleMask);
                foreach(RaycastHit hit in hits)
                {
                    // For reach obstacle, reduce the volume by 0.5
                    volumeFactor /= 2.0f;
                }               
                // Assign the volume 
                currentSoundInstance.GetComponent<AudioSource>().volume = volumeFactor;
                // Play the sound
                currentSoundInstance.GetComponent<AudioSource>().PlayOneShot(clipToUse);
                // Destroy the sphere when the clip ends
                Destroy(currentSoundInstance, clipToUse.length);
            }
        }
    }
}
