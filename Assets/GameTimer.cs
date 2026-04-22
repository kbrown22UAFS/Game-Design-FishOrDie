using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;

    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        UpdateTimerUI();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            LoseGame();
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time until you DIE: " + Mathf.Ceil(timeRemaining).ToString();
    }

    void LoseGame()
    {
        if (ScoreManager.gameEnded) return;
        ScoreManager.gameEnded = true;
        SceneManager.LoadScene("YouLoseScene");
    }
}
