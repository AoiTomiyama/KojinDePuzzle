using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutIn : MonoBehaviour
{
    [Header("�J�n����X���W")]
    [SerializeField]
    private int _startPosX = 3;
    [Header("��~����X���W")]
    [SerializeField]
    private int _stopPosX = 35;
    [Header("�J�b�g�C���̈ړ����x")]
    [SerializeField]
    private float _cutInMoveSpeed = 0.8f;
    [Header("�V�[���J�n���ɋN�����邩")]
    [SerializeField]
    bool _activateOnStart = false;

    private void Start()
    {
        if (_activateOnStart)
            StartCoroutine(ShowView());
    }
    public void OnButtonPressed()
    {
        GameObject.Find("MainMenu").SetActive(false);
        StartCoroutine(HideViewAndSceneMove("Main"));
    }
    public IEnumerator ShowView()
    {
        transform.position = new Vector2(_startPosX, transform.position.y);
        while (transform.position.x < _stopPosX)
        {
            transform.position += Vector3.right * _cutInMoveSpeed;
            yield return null;
        }
    }

    public IEnumerator HideViewAndSceneMove(string sceneName)
    {
        LoadingCircle.SceneNameToMove = sceneName;
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector2(_stopPosX, transform.position.y);
        while (transform.position.x > _startPosX)
        {
            transform.position += Vector3.left * _cutInMoveSpeed;
            yield return null;
        }
        Debug.Log(sceneName);
        SceneManager.LoadScene(1);
    }
}
