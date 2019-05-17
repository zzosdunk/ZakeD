using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Text scoreText;
    public static int score;

    public Text highScoreText;
    public static int highScore;
    private static int currentScore;

    public GameObject gameOverUI;
    public bool isGameOver;

    public GameObject pushTimeText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start () {
        highScoreText.text = "Highscore: " + highScore;
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        currentScore = PlayerPrefs.GetInt("currentScore", currentScore);
        
    }

	void Update () {
        if (GameManager.Instance.isGameOver == true && Input.GetButton("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("reload scene");
        }
        HighScore();
        scoreText.text = "Score: " + score;
    }
    public void AddScore(bool isGameOver)
    {
        if (isGameOver==true)
        {
            score++;
            currentScore = score;
            PlayerPrefs.SetInt("currentScore", currentScore);
            scoreText.text = "Score: " + score;
            HighScore();
        } else
        {
            score = 0;
            scoreText.text = "Score: " + score;
        }
        
    }
    public void HighScore()
    {
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("highScore", highScore);
        }
        highScoreText.text = "Highscore: " + highScore.ToString();
    }
    public void EndGame()
    {
        pushTimeText.SetActive(false);
        gameOverUI.SetActive(true);
        AddScore(false);
        isGameOver = true;

    }
}
