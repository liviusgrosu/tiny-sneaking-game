using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    private EntitySoundController _soundController;

    private void Start()
    {
        _soundController = GetComponent<EntitySoundController>();
    }

    public void PlayGrabCoin()
    {
        _soundController._gameSoundController.PlaySpecificSound("Grab Coins", transform.position);
    }

    public void PlayHitSound()
    {
        _soundController._gameSoundController.PlayRandomFromSet("Player Hit", transform.position);
    }
}
