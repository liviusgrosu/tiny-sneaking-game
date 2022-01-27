using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySoundController : MonoBehaviour
{
    [HideInInspector] public GameSoundController _gameSoundController;
    [SerializeField] private LayerMask _floorMask;

    private void Start()
    {
        _gameSoundController = GameObject.Find("Game Manager").GetComponent<GameSoundController>();
    }

    public void PlayFootstep()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        // Get the material under the player
        if (Physics.Raycast(ray, out hit, 3000.0f, _floorMask))
        {
            // Get the material name and play that appropriate material sound
            _gameSoundController.PlaySpecificSound($"{hit.collider.tag} Footstep", transform.position);
        }
    }
}
