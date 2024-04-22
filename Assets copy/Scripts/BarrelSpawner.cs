using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject barrel;
    public float minTime = 2f;
    public float maxTime = 4f;
    private Animator _donkeyAnimator;
    [SerializeField] private int magicBarrelsRat;
    private const float THROW_ANIMATION_TIME = 1;
    private const float X_VEC = -1.85f;
    private const float Y_VEC = 2f;
    private Vector3 _startFirstPosition = new Vector3(X_VEC, Y_VEC, 0);


    private void Awake()
    {
        _donkeyAnimator = transform.GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(Throwmenegment());
        
    }

    private void Update()
    {
        _donkeyAnimator.SetBool("throw", false);
    }

    private IEnumerator Throw()
    {
        _donkeyAnimator.SetBool("throw", true);

        yield return new WaitForSeconds(THROW_ANIMATION_TIME);
        Instantiate(barrel, _startFirstPosition, Quaternion.identity);
    }

    private IEnumerator Throwmenegment()

    { 
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        StartCoroutine(Throw());
        StartCoroutine(Throwmenegment()); //recursively

    }

}