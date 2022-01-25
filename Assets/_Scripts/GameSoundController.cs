using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour
{
    [SerializeField] private GameObject SoundInstance;
    // [SerializeField] private AudioClip WoodStep, CarpetStep, StoneStep;

    [Serializable]
    public struct Clips
    {
        public string name;
        public AudioClip audioClip;
        public int size;
    }

    [SerializeField] private Clips[] _clips;

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
                GameObject currentSoundInstance = Instantiate(SoundInstance, position, Quaternion.identity);
                currentSoundInstance.transform.localScale = currentClip.size * Vector3.one;
                // Play the sound
                currentSoundInstance.GetComponent<AudioSource>().PlayOneShot(clipToUse);
                // Destroy the sphere when the clip ends
                Destroy(currentSoundInstance, clipToUse.length);
            }
        }
    }
}
