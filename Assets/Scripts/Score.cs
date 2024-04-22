using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Score : MonoBehaviour
{
    private const float MIN_HIGH = -3.57f;
    private float _maxClimb;
    private const float DISTANCE = 0.5f;
    private const int JUMP_SCORE = 100;
    private GameObject _scoreLabal;
    private bool _startoJump;
    [SerializeField] private LayerMask isBarrel;


    private void Awake()
    {
        _maxClimb = MIN_HIGH;
        _scoreLabal = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        if (transform.position.y > _maxClimb + 0.3)
        {
            _maxClimb = transform.position.y;
            Game.instance.UpdateScore(50);
        }

        CheckJumpAboveBarrel();
    }

    private void CheckJumpAboveBarrel()
        //check if jump above barrel
    {
        RaycastHit2D jumpAboveBarrel = Physics2D.Raycast(transform.position,
            Vector2.down, DISTANCE, isBarrel);
        if (jumpAboveBarrel.collider != null)
        {
            if (_startoJump)
            {
                AudioMeneger.Audio.Play(AudioMeneger.Audio.jumpOverClip);
                _scoreLabal.SetActive(true);
                Game.instance.UpdateScore(JUMP_SCORE);
                _startoJump = false;
                StartCoroutine(nameof(HandleLabel));
            }
        }
        else
        {
            _startoJump = true;
        }
    }

    private IEnumerator HandleLabel()
    {
        yield return new WaitForSeconds(0.3f);
        _scoreLabal.SetActive(false);
    }
    
}

