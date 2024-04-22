using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HammerThrow : MonoBehaviour
{
    public GameObject hammerPrefab;
    private Player _player;
    public float throwForce = 5f;
    private readonly Vector2 _dirLeft = new(-1, 0.2f);
    private readonly Vector2 _dirRight = new(1, 0.2f);
    public float engularVel = 700f;
    private int _numOfHammers;
    public TextMeshProUGUI hammers;
    public List<GameObject> hammerList;
    private GameObject _projectile;
    private Rigidbody2D _rb;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _numOfHammers = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _numOfHammers > 0)
        {
            _numOfHammers--;
            ThrowHammer();
            UpdateHammers();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Hammerpoint"))
        {
            _numOfHammers += 1;
            collision.gameObject.SetActive(false);
            UpdateHammers();
        }
    }

    private void ThrowHammer()
    {
        _projectile = Instantiate(hammerPrefab, transform.position, Quaternion.identity);
        _rb = _projectile.GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0.15f;

        if (_player.GetFace())
        {
            _rb.velocity = throwForce * _dirRight;
            _rb.angularVelocity = -engularVel;
        }
        else
        {
            _rb.velocity = throwForce * _dirLeft;
            _rb.angularVelocity = engularVel;
        }
    }

    private void UpdateHammers()
    {
        hammers.text = "Hammers: " + _numOfHammers.ToString();
    }

    public void Reset()
    {
        _numOfHammers = 1;
        foreach (var temp in hammerList)
        {
            temp.gameObject.SetActive(true);
            UpdateHammers();
        }
    }
}