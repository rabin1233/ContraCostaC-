using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour
{
    Text Highscore;
    void OnEnable()
    {
        Highscore = GetComponent<Text>();
        Highscore.text = "Highscore: " + PlayerPrefs.GetInt("HighScore").ToString();
        System.Console.WriteLine("Highscore");
    }
}
