using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���[�f�B���O��ʂ̉E���ɂ��邭�邭�����̐���ƁA���[�h��ɂǂ̃V�[���Ɉړ����邩�̃X�N���v�g�B
/// </summary>
public class LoadingCircle : MonoBehaviour
{
    [Header("��]���x")]
    [SerializeField]
    float _rotateSpeed = 1;
    float _rotateZ;
    private static string _sceneNameToMove = "Title";

    public static string SceneNameToMove { set => _sceneNameToMove = value; }

    private void Start()
    {
        Time.timeScale = 1;
        Debug.Log($"Wait 2 second and load scene, sceneName is {_sceneNameToMove}");
        //���[�f�B���O��ʂɈړ����Ă���2�b���LoadScene�֐������s�B
        Invoke(nameof(LoadScene), 2);
    }


    void Update()
    {
        _rotateZ += _rotateSpeed;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
    }
    /// <summary>
    /// ���̃V�[�������[�h����B
    /// </summary>
    void LoadScene()
    {
        Debug.Log("Scene successfully loaded!");
        SceneManager.LoadScene(_sceneNameToMove);
    }
}
