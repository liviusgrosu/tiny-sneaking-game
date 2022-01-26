using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    private EntitySoundController _soundController;
    [SerializeField] private LayerMask _floorMask;

    private void Start()
    {
        _soundController = GetComponent<EntitySoundController>();
    }

    public void PlayGrabCoin()
    {
        _soundController._gameSoundController.PlaySound("Grab Coins", transform.position);
    }
}
