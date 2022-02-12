using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameSession : MonoBehaviour
{
    int playerScore;

    private void Awake()
    {
        ScoreSingleton();
    }
    public void ScoreSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public int GetScore() { return playerScore; }
    public void AddToScore(int points) { playerScore += points; }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
