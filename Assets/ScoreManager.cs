using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;

    public TextMeshProUGUI scoreText;
    public int totalFish = 3;

    void Start()
    {
        UpdateScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        totalFish--;

        UpdateScore();

        if (totalFish <= 0)
        {
            GameOver();
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
