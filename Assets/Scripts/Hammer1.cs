using UnityEngine;

public class Hammer1 : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckIfStatic();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Barrel"))
        {
            HameerDie();
        }
        else if (collision.transform.CompareTag("Wall"))
        {
            HameerDie();
        }
    }

    private void HameerDie()
    {
        gameObject.SetActive(false);
        Destroy(this);
    }

    private void CheckIfStatic()
    {
        if (_rigidbody2D.velocity.magnitude < 0.3)
        {
            HameerDie();
        }
    }
}