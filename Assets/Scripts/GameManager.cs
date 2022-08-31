using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI percentText;
    public GameObject tapToPlayText;
    public static GameManager Instance;
    public int score = 0;
    public int highScore = 0;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if(MovingCube.CurrentCube != null)
                MovingCube.CurrentCube.Stop();
            
            FindObjectOfType<CubeSpawner>().SpawnCube();

            if (tapToPlayText != null)
                Destroy(tapToPlayText.gameObject);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

