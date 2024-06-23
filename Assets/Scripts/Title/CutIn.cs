using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�n���ƃV�[���ړ����ɃJ�b�g�C����}������X�N���v�g�B
/// �V�[���ړ�����ۂ͕K���������o�R���邱�ƁB
/// </summary>
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
    /// <summary>
    /// �^�C�g���̃X�^�[�g�{�^���������ꂽ�Ƃ��̓���B
    /// </summary>
    public void OnButtonPressed()
    {
        GameObject.Find("MainMenu").SetActive(false);
        StartCoroutine(HideViewAndSceneMove("Main"));
    }
    /// <summary>
    /// ��ʂ��B��Ă����Ԃ���V���b�ƃJ�b�g�C������蕥���B
    /// </summary>
    public IEnumerator ShowView()
    {
        transform.position = new Vector2(_startPosX, transform.position.y);
        while (transform.position.x < _stopPosX)
        {
            transform.position += Vector3.right * _cutInMoveSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// ��ʂ��V���b�ƉB���J�b�g�C������ꂽ�̂��A�V�[�����ړ�����B
    /// </summary>

    public IEnumerator HideViewAndSceneMove(string sceneName)
    {
        Debug.Log("Hide view");
        LoadingCircle.SceneNameToMove = sceneName;
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
