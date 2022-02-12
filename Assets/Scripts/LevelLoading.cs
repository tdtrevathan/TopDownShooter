using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] float delayTime = 2f;
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        
        SceneManager.LoadScene("Game");

        GameSession gameSession = FindObjectOfType<GameSession>();
        if(!gameSession){ return; }

        gameSession.ResetGame();
    }
    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOver()); 
       
    }

    public void LoadWin()
    {
        StartCoroutine(DelayWin());

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("GameOver");

    }
    private IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(delayTime +3f);
        SceneManager.LoadScene("Win");

    }
}
