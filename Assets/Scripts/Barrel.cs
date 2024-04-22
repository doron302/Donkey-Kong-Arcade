using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    Rigidbody2D _rb;
    public float speed = 10;
    private Vector2 _velocity;
    private bool _nearLadder;
    Collider2D _currentGround;
    private bool _touchGround;
    private readonly float GRAVITY_DOWN_LADDER = 0.5f;
    private float REGULAR_GRAVITY = 1f;
    private Animator _anim;
    private bool _downLadder;
    private static readonly int Down = Animator.StringToHash("down");
    private SpriteRenderer _pSpriteRenderer;
    private bool _faceleft = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _velocity = new Vector2(speed, 0);
        _pSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _rb.velocity = _velocity;
    }

    private void Update()
    {
        if (Game.instance.Get_die())
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if ((_touchGround && _downLadder == false) ||
            (_rb.velocity.magnitude == 0))
        {
            _rb.velocity = _velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Turn"))
        {
            _velocity = -_velocity;
            _rb.velocity = _velocity;
            _pSpriteRenderer.flipX = _faceleft;
            _faceleft = !_faceleft;
            
            
        }
        else if (collision.transform.CompareTag("Ladder"))
        {
            _nearLadder = true;
            DecideToGoDownLadder();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("HotOil"))
        {
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Ground"))
        {
            _touchGround = true;
            _currentGround = collision.gameObject.GetComponent<Collider2D>();
        }
        else if (collision.transform.CompareTag("Hammer"))
        {
            Destroy(gameObject);
            AudioMeneger.Audio.Play(AudioMeneger.Audio.jumpOverClip);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _touchGround = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void DecideToGoDownLadder()
    {
        if (_nearLadder && Random.Range(0, 100) < 30) // 30% chance to go down the ladder
        {
            _touchGround = false;
            StartCoroutine(GoDownTheLadder());
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator GoDownTheLadder()
    {
        Physics2D.IgnoreCollision(_currentGround, GetComponent<Collider2D>(), true);
        _downLadder = true;
        _anim.SetBool(Down, true);
        _rb.gravityScale = GRAVITY_DOWN_LADDER;
        _rb.velocity = Vector2.down;
        yield return new WaitUntil(TouchGround);
        EndLadder();
    }

    private bool TouchGround()
    {
        return _touchGround;
    }

    private void EndLadder()
    {
        _rb.gravityScale = REGULAR_GRAVITY;
        _rb.velocity = -_velocity;
        _velocity = -_velocity;
        _anim.SetBool(Down, false);
        _downLadder = false;
    }
}