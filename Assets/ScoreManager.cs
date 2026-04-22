using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public static bool gameEnded = false;

    public TextMeshProUGUI scoreText;
    public int totalFish = 3;

    // Set this in Inspector (last level number)
    public int finalLevelIndex = 2;

    void Start()
    {
        gameEnded = false;

        //Automatically count fish in scene
        totalFish = GameObject.FindGameObjectsWithTag("Fish").Length;

        UpdateScore();
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;

        score += amount;
        totalFish--;

        UpdateScore();

        if (totalFish <= 0)
        {
            LoadNextLevel();
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Fish Caught: " + score;
    }

    void LoadNextLevel()
    {
        if (gameEnded) return;

        gameEnded = true;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex >= finalLevelIndex)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
    }
}
