using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game instance;
    private int _lives = 3;
    private int _score;
    private bool _die;
    public Paulina paulina;
    public List<GameObject> liveslist;
    public TextMeshProUGUI scoreLabel;
    private TextMeshProUGUI _highscoreLabel;
    public GameObject player;
    private Vector2 _startPos;
    public Player playerscript;
    public HammerThrow thrower;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Singleton violation");
            return;
        }

        instance = this;
        _startPos = player.transform.position;
    }


    private void Start()
    {
        UpdateScore(0);
    }

    private void Update()
    {
        if (IsWon)
        {
            Invoke(nameof(GoToEndScene), 2f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    private void GoToEndScene()
    {
        SceneManager.LoadScene("WinScene");
    }


    public void Die()
    {
        AudioMeneger.Audio.Play(AudioMeneger.Audio.dieClip);
        _lives--;
        liveslist[_lives].GetComponent<Renderer>().enabled = false;
        _die = true;
        paulina.FrezzePaulina(true);
        thrower.Reset();
    }

    public void Loadsomescene()
    {
        if (_lives <= 0)
        {
            GameOver();
        }
        else
        {
            Resetscene();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("OpenScene");
    }

    public bool Get_die()
    {
        return _die;
    }


    public void UpdateScore(int amount)
    {
        _score += amount;
        scoreLabel.text = "Score: " + _score;
    }

    private void Resetscene()
    {
        _score = 0;
        UpdateScore(0);
        player.transform.position = _startPos;
        _die = false;
        playerscript.Restart();
        thrower.Reset();
        IsWon = false;
        paulina.FrezzePaulina(false);
    }

    public bool IsWon { get; set; }
}