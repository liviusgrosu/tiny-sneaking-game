using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    public enum Type
    {
        Enter,
        Exit
    }; 

    [SerializeField] private Type _type;
    [SerializeField] private string _words;
    
    [SerializeField] private Text _text;
    [SerializeField] private Image _background;
    private float _time;
    [SerializeField] private float _fadeTime = 1.0f;
    public bool _isFading;
    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || _used)
        {
            // Ignore if not player or already used
            return;
        }

        // Start fading
        _time = 0.0f;
        _isFading = true;
    }

    private void Update()
    {
        if (_isFading && !_used)
        {
            Debug.Log("still here");
            _time += Time.deltaTime;
            float fadeFactor = (_type == Type.Enter) ? _time / _fadeTime : 1 - (_time / _fadeTime);

            
            if (_type == Type.Enter && fadeFactor > 0.9f)
            {
                fadeFactor = 1.0f;
                _used = true;
            }
            else if (_type == Type.Exit && fadeFactor < 0.1f)
            {
                fadeFactor = 0.0f;
                _used = true;
            }

            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, fadeFactor);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, fadeFactor);
        }
    }
}