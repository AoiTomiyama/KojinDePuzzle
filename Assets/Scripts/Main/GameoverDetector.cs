using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���I�[�o�[�̏����𖞂����Ă��邩�����m����X�N���v�g�B
/// </summary>
public class GameoverDetector : MonoBehaviour
{
    [SerializeField]
    GameObject gameoverPanel, statsPanel;

    private bool _isGameOver = false;
    string _currentSceneName;
    bool _isCoroutineStarted = false;
    AudioSource _aus;

    CutIn cutIn;

    public bool IsGameOver { get => _isGameOver; }

    private void Start()
    {
        Time.timeScale = 1;
        _currentSceneName = SceneManager.GetActiveScene().name;
        gameoverPanel.SetActive(false);
        cutIn = FindObjectOfType<CutIn>();
        _aus = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //L�L�[�ŋ����I�ɃQ�[���I�[�o�[��Ԃ������i�f�o�b�O�p�j�i�r���h���ɂ͏������Ɓj
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    _isGameOver = false;
        //    Time.timeScale = 1;
        //    gameoverPanel.SetActive(false);
        //    statsPanel.SetActive(true);
        //}

        if (Mathf.Round(Generate.Timer) <= 0 && IsGameOver == false)
        {
            Gameover();
        }

        if (IsGameOver == true && _isCoroutineStarted == false)
        {
            if (Input.GetButtonDown("Restart"))
            {
                Debug.Log("Reload this scene");
                StartCoroutine(cutIn.HideViewAndSceneMove(_currentSceneName));
                _isCoroutineStarted = true;
            }
            else if (Input.GetButtonDown("GoTitle"))
            {
                Debug.Log("Move to title");
                StartCoroutine(cutIn.HideViewAndSceneMove("Title"));
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
    /// �Q�[�������Ԃ��~�߂āAGameover��ʂ�\�����AStats���B���B
    /// </summary>
    void Gameover()
    {
        _aus.Play();
        Time.timeScale = 0;
        _isGameOver = true;
        gameoverPanel.SetActive(true);
        statsPanel.SetActive(false);
    }
}
