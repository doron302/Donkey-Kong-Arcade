using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private float moveSpeed = 10f;
    private float _moveDir;
    private float _climbDir;
    public Animator anim;
    private bool _jumpPressed;
    private float _jumpYVel;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _moveVel;
    private bool _faceingrigih;
    private IsGrounded _isGrounded;
    private bool _climbing;
    public float climbForce = 100f;
    private bool _canClimb;
    private float _topLadeer;
    private float _buttomLadder;
    private Collider2D _coliider;
    private float _xLadder;
    private readonly int _groundLayerinx = 6;
    private readonly int _playerLayerinx = 9;
    private const float TOPOFFSET = 0.1f;
    private const float BOTTOMOFFSET = 0.35f;
    private SpriteRenderer _pSpriteRenderer;
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Grounded = Animator.StringToHash("grounded");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Endclimb = Animator.StringToHash("Endclimb");
    private static readonly int Climb = Animator.StringToHash("Climb");

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _isGrounded = GetComponentInChildren<IsGrounded>();
        _coliider = GetComponent<Collider2D>();
        _pSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetInput();
        ClimbingAnimations();
    }

    private void FixedUpdate()
    {
        Move();
        HandleJump();
    }

    private void HandleJump()
    {
        if (_isGrounded.Is_Grounded && _jumpPressed)
        {
            AudioMeneger.Audio.Play(AudioMeneger.Audio.jumpClip);
            

            _jumpYVel = CalculateJumpVel(jumpHeight);
            _jumpPressed = false;

            _moveVel = _rigidbody2D.velocity;
            _moveVel.y = _jumpYVel;
            _rigidbody2D.velocity = _moveVel;
            anim.SetTrigger(Jump);
        }

    }

    private void Move()
    {
        if (_climbing)
        {
            HendleClimbing();
        }
        else
        {
            HendeleMovement();
        }
        //apply movement
        _rigidbody2D.velocity = _moveVel;
  
    }

    private void HendleClimbing()
    {
        anim.SetBool(Walk, false);
        _moveVel.y = _climbDir * climbForce * Time.fixedDeltaTime;
        _moveVel.x = 0;
    }

    private void HendeleMovement()
    {
        _moveVel = _rigidbody2D.velocity;
        _moveVel.x = _moveDir * moveSpeed * Time.fixedDeltaTime;

        anim.SetBool(Grounded, _isGrounded.Is_Grounded);

        if (Mathf.Abs(_moveVel.x) > 0) //on moveing
        {
            switch (_moveVel.x)
            {
                case > 0 when !_faceingrigih:
                    _pSpriteRenderer.flipX = true;
                    _faceingrigih = true;
                    break;
                case < 0 when _faceingrigih:
                    _pSpriteRenderer.flipX = false;
                    _faceingrigih = false;
                    break;
            }

            if (_isGrounded.Is_Grounded)
            {
                anim.SetBool(Walk, true);

                if (!AudioMeneger.Audio.Is_Playing() && !Game.instance.Get_die())
                {
                    AudioMeneger.Audio.Play(AudioMeneger.Audio.walkClip);
                }
            }
            else
            {
                anim.SetBool(Walk, false);
            }
        }
        else
        {
            anim.SetBool(Walk, false);
        }
    }


    private float CalculateJumpVel(float height)
    {
        return MathF.Sqrt((-2 * _rigidbody2D.gravityScale * Physics2D.gravity.y * height));
    }

    void GetInput()
    {
        if (Game.instance.Get_die() || Game.instance.IsWon)
        {
            _moveDir = 0;
            return;
        }   

        if (_canClimb)
        {
            GetClimbingInput();
            if (_climbing)
            {
                anim.SetBool(Walk,false);

            }
        }

        _moveDir = Input.GetAxisRaw("Horizontal"); // takes move input
        if (_isGrounded.Is_Grounded&!_climbing)
        {
            _jumpPressed |= Input.GetKey(KeyCode.Space); // takes input for jump using space
        }   
    }

    private void GetClimbingInput()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (_climbing == false)
            {
                if (Math.Abs(_coliider.bounds.min.y - _topLadeer ) < TOPOFFSET & Input.GetAxis("Vertical") < 0)
                {
                    StartLadder();
                }
                if (_coliider.bounds.min.y - _buttomLadder < 0 & Input.GetAxis("Vertical") > 0)
                {
                    StartLadder();
                }
            }

            _climbDir = Input.GetAxis("Vertical");
            check_where_in_ladders();
        }
        else
        {
            _climbDir = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("FakeLadder"))
        {
            _canClimb = true;
            _topLadeer = collision.GetComponent<Collider2D>().bounds.max.y;
            _buttomLadder = collision.GetComponent<Collider2D>().bounds.min.y;
            _xLadder = collision.transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("FakeLadder"))
        {
            _canClimb = false;
            Physics2D.IgnoreLayerCollision(_groundLayerinx, _playerLayerinx, false);
            End_ladder();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            if (_climbing)
            {
                Physics2D.IgnoreLayerCollision(_groundLayerinx, _playerLayerinx, true);
            }
        }
        else if (collision.transform.CompareTag("Barrel"))
        {
            anim.SetBool(Die, true);
            Game.instance.Die();
        }
    }

    private void StartLadder()
    {
        _climbing = true;
        _rigidbody2D.gravityScale = 0;
        var transform1 = transform;
        transform1.position = new Vector2(_xLadder, transform1.position.y);
        anim.SetBool(Endclimb, false);
    }

    private void End_ladder()
    {
        _rigidbody2D.gravityScale = 1;
        _climbing = false;
        anim.SetBool(Climb, false);
        anim.SetBool(Endclimb, true);
        Physics2D.IgnoreLayerCollision(_groundLayerinx, _playerLayerinx, false);
    }


    private void ClimbingAnimations()
    {
        if (_climbing && _rigidbody2D.velocity.y != 0)
        {
            anim.SetBool(Climb, true);
        }
        else
        {
            anim.SetBool(Climb, false);
            anim.SetBool(Grounded, _isGrounded.Is_Grounded);
        }
    }


    public void Restart()
    {
        anim.SetBool(Die, false);
    }

    private void check_where_in_ladders()
    {
        if (Math.Abs(_coliider.bounds.min.y - _topLadeer) < TOPOFFSET)
        {
            Physics2D.IgnoreLayerCollision(_groundLayerinx, _playerLayerinx, true);
        }

        if ((_buttomLadder - _coliider.bounds.min.y) > BOTTOMOFFSET && _climbDir < 0)
        {
            End_ladder();
        }

        if (Math.Abs(_coliider.bounds.min.y-_topLadeer) < TOPOFFSET && _climbDir > 0)
        {
            End_ladder();
        }
    }

    public bool GetFace()
    {
        return _faceingrigih;
    }

    public void EndOfDieAnim()
    {
        Game.instance.Loadsomescene();
    }
}