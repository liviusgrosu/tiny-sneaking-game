using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PickUpLoot : MonoBehaviour
{
    private LootManager _lootManager;
    private List<Transform> _availableLoot = new List<Transform>();
    private Animator _animator;
    [SerializeField] private LayerMask _lootMask;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _lootManager = GameObject.Find("Game Manager").GetComponent<LootManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        // When its close to the player, add the loot to the list of available loot to pickup
        if (col.tag == "Loot" && !_availableLoot.Contains(col.transform))
        {
            _availableLoot.Add(col.transform);
            col.GetComponent<Outline>().OutlineWidth = 1.0f;
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        // When its far from the player, remove the loot to the list of available loot to pickup
        if (col.tag == "Loot" && _availableLoot.Contains(col.transform))
        {
            _availableLoot.Remove(col.transform);
            col.GetComponent<Outline>().OutlineWidth = 0.0f;
        }
    }

    void Update()
    {
        // Clicking on close by loot will add to the score
        if (Input.GetMouseButtonDown(0))
        {   
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if object clicked is a loot object
            if (Physics.Raycast(mouseRay, out hit, 3000f, _lootMask))
            {
                if (_availableLoot.Contains(hit.collider.transform))
                {
                    _animator.SetTrigger("Grab");
                    // Get direction of loot from player and normalize it
                    Vector3 lootDirection =  hit.collider.transform.position - transform.position;
                    lootDirection = new Vector3(lootDirection.x, transform.position.y, lootDirection.z);
                    // Look at the look when grabbing it
                    transform.rotation = Quaternion.LookRotation(lootDirection, Vector3.up);

                    // Add the loot to the score and destroy it
                    _lootManager.AddLoot(hit.collider.GetComponent<LootObject>().Score);
                    _availableLoot.Remove(hit.collider.transform);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
