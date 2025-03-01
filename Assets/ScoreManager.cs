using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (scoreText != null) // Prevents NullReferenceException
        {
            scoreText.text = "Score: " + CoinCollector.score;
        }
    }
}
