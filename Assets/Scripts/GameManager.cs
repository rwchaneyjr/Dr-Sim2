using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int sickTiles = 0;

    [Header("Game Timer")]
    public float gameTime = 120f;
    private float currentTime;

    [Header("Score")]
    public int score = 0;
    private int combo = 0;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text scoreText;

    private bool gameOver = false;

    void Start()
    {
        sickTiles = 0;
        currentTime = gameTime;
        gameOver = false;

        Debug.Log("gameTime in Inspector = " + gameTime);
        Debug.Log("currentTime after Start = " + currentTime);
    }

    void Update()
    {
        if (gameOver) return;

        currentTime -= Time.deltaTime;

        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(currentTime);

        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        if (scoreText != null)
            scoreText.text = "Score: " + score + "  Combo: x" + combo;
    }

    public void AddScore(int amount)
    {
        if (amount > 0)
        {
            combo++;
            score += amount * combo;
        }
        else
        {
            combo = 0;
            score += amount;
        }
    }

    void EndGame()
    {
        gameOver = true;
        Debug.Log("GAME OVER - Final Score: " + score);
    }
}