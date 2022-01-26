using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSoundController : MonoBehaviour
{
    private EntitySoundController _soundController;
    [SerializeField] private LayerMask _floorMask;

    private void Start()
    {
        _soundController = GetComponent<EntitySoundController>();
    }
}
