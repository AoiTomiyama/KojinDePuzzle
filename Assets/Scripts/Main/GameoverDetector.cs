using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverDetector : MonoBehaviour
{
    [SerializeField]
    GameObject gameoverPanel, statsPanel;

    [NonSerialized]
    public bool _isGameOver = false;
    int sceneIndex;

    private void Start()
    {
        Time.timeScale = 1;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameoverPanel.SetActive(false);
    }
    private void Update()
    {
        //L�L�[�ŋ����I�ɃQ�[���I�[�o�[��Ԃ������i�f�o�b�O�p�j�i�r���h���ɂ͏������Ɓj
        if (Input.GetKeyDown(KeyCode.L))
        {
            _isGameOver = false;
            Time.timeScale = 1;
            gameoverPanel.SetActive(false);
            statsPanel.SetActive(true);
        }

        if (Mathf.Round(Generate.timer) <= 0 && _isGameOver == false)
        {
            Gameover();
        }

        if (_isGameOver == true)
        {
            if (Input.GetButtonDown("Restart"))
            {
                SceneManager.LoadScene(sceneIndex);
            }
            else if (Input.GetButtonDown("GoTitle"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Ball") && _isGameOver == false)
        {
            Gameover();
        }
    }

    /// <summary>
    /// �Q�[�������Ԃ��~�߂āAGameover��ʂ�\�����AStats���B���B
    /// </summary>
    void Gameover()
    {
        Time.timeScale = 0;
        _isGameOver = true;
        gameoverPanel.SetActive(true);
        statsPanel.SetActive(false);
    }
}
