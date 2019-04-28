using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore(int addition)
    {
        score += addition;
        scoreText.text = "Score: " + score.ToString();
    }

    public void Reset()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }
}
