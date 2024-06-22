using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverDetector : MonoBehaviour
{
    [SerializeField]
    GameObject gameoverPanel, statsPanel;

    private bool _isGameOver = false;
    string _currentSceneName;
    bool _isCoroutineStarted = false;

    CutIn cutIn;

    public bool IsGameOver { get => _isGameOver;}

    private void Start()
    {
        Time.timeScale = 1;
        _currentSceneName = SceneManager.GetActiveScene().name;
        gameoverPanel.SetActive(false);
        cutIn = FindObjectOfType<CutIn>();
    }
    private void Update()
    {
        //Lキーで強制的にゲームオーバー状態を解除（デバッグ用）（ビルド時には消すこと）
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    _isGameOver = false;
        //    Time.timeScale = 1;
        //    gameoverPanel.SetActive(false);
        //    statsPanel.SetActive(true);
        //}

        if (Mathf.Round(Generate.timer) <= 0 && IsGameOver == false)
        {
            Gameover();
        }

        if (IsGameOver == true && _isCoroutineStarted == false)
        {
            if (Input.GetButtonDown("Restart"))
            {
                Debug.Log("Reload this scene");
                StartCoroutine(cutIn.SceneMove(_currentSceneName));
                _isCoroutineStarted = true;
            }
            else if (Input.GetButtonDown("GoTitle"))
            {
                Debug.Log("Move to title");
                StartCoroutine(cutIn.SceneMove("Title"));
                _isCoroutineStarted = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Ball") && IsGameOver == false)
        {
            Gameover();
        }
    }

    /// <summary>
    /// ゲーム内時間を止めて、Gameover画面を表示し、Statsを隠す。
    /// </summary>
    void Gameover()
    {
        Time.timeScale = 0;
        _isGameOver = true;
        gameoverPanel.SetActive(true);
        statsPanel.SetActive(false);
    }
}
