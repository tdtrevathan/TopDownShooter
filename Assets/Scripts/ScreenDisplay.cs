using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScreenDisplay : MonoBehaviour
{
    GameSession gameSession;
    TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
       

        gameSession = FindObjectOfType<GameSession>();
       

    }
    private void Update()
    {
            scoreText.text = gameSession.GetScore().ToString();

    }


}
