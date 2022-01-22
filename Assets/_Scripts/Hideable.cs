using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable : MonoBehaviour
{
    [SerializeField] private Renderer _rend;

    public void ToggleState(bool state)
    {
        _rend.enabled = state;
    }
}