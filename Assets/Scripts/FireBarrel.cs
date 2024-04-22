using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarrel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Sprite[] fire;
    private SpriteRenderer _spriteRenderer;
    private const float Y_POS = 0.5f;
    private const float GAP = 0.2f;
    private float _timer;
    private int _index;
    private Vector3 _startPos;
    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _timer = Time.time;
        _startPos = transform.position;
        _startPos.y -= Y_POS;
    }

    private void Update()
    {
        if (Time.time - _timer > GAP && fire.Length != 0)
        {
            _spriteRenderer.sprite = fire[_index];
            _index++;
            if (_index == fire.Length)
                _index = 0;
            _timer = Time.time;
        }
    }
}
