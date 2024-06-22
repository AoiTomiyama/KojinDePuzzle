using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ローディング画面の右下にあるくるくる回るやつの制御と、ロード後にどのシーンに移動するかのスクリプト。
/// </summary>
public class LoadingCircle : MonoBehaviour
{
    [Header("回転速度")]
    [SerializeField]
    float _rotateSpeed = 1;
    float _rotateZ;
    private static string _sceneNameToMove = "Title";

    public static string SceneNameToMove { set => _sceneNameToMove = value; }

    private void Start()
    {
        Time.timeScale = 1;
        Debug.Log($"Wait 2 second and load scene, sceneName is {_sceneNameToMove}");
        //ローディング画面に移動してから2秒後にLoadScene関数を実行。
        Invoke(nameof(LoadScene), 2);
    }


    void Update()
    {
        _rotateZ += _rotateSpeed;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
    }
    /// <summary>
    /// 次のシーンをロードする。
    /// </summary>
    void LoadScene()
    {
        Debug.Log("Scene successfully loaded!");
        SceneManager.LoadScene(_sceneNameToMove);
    }
}
