using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterWon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReLoadscene());
    }
    public IEnumerator ReLoadscene()
    {
        yield return new WaitForSeconds(2f);
        Gamereloadver();

    }
    public void Gamereloadver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
