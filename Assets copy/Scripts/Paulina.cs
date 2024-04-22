using System.Collections;
using UnityEngine;

public class Paulina : MonoBehaviour
{
    [SerializeField] private Sprite heart;
    private Animator _paulinaAnim;
    private SpriteRenderer _childrenderer;
    private bool _win;
    private static readonly int End = Animator.StringToHash("end");

    private void Start()
    {
        _paulinaAnim = GetComponent<Animator>();
        _paulinaAnim.SetBool(End, false);
        _childrenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_win)
            _childrenderer.enabled =
                _paulinaAnim.GetCurrentAnimatorStateInfo(0)
                    .IsName("Help");
        else
        {
            _childrenderer.enabled = true;
            _childrenderer.sprite = heart;
            StartCoroutine(EndScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _win = true;
            _paulinaAnim.SetBool(End, true);
            AudioMeneger.Audio.Play(AudioMeneger.Audio.winClip);
            Game.instance.IsWon = true;
        }
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(1);
    }

    public void FrezzePaulina(bool setter)
    {
        _paulinaAnim.SetBool(End, setter);
    }
}