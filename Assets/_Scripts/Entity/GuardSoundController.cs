using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSoundController : MonoBehaviour
{
    private EntitySoundController _soundController;

    private void Start()
    {
        _soundController = GetComponent<EntitySoundController>();
    }

    public void PlayAttackSound()
    {
        _soundController._gameSoundController.PlayRandomFromSet("Guard Attack", transform.position);
    }

    public void PlaySightSound()
    {
        _soundController._gameSoundController.PlayRandomFromSet("Guard Sight", transform.position);
    }

    public void PlaySearchSound()
    {
        _soundController._gameSoundController.PlayRandomFromSet("Guard Search", transform.position);
    }

    public void PlayAlertSound()
    {
        _soundController._gameSoundController.PlayRandomFromSet("Guard Alert", transform.position);
    }
}
