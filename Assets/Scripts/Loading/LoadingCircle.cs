using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCircle : MonoBehaviour
{
    [Header("‰ñ“]‘¬“x")]
    [SerializeField]
    float _rotateSpeed = 1;
    float _rotateZ;
    public static string _sceneNameToMove;

    private void Start()
    {
        Time.timeScale = 1;
        Debug.Log($"Wait 2 second and load Scene, seneIndex is {_sceneNameToMove}");
        Invoke(nameof(LoadScene), 2);
    }


    void Update()
    {
        _rotateZ += _rotateSpeed;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
    }
    void LoadScene()
    {
        Debug.Log("Scene successfully loaded!");
        SceneManager.LoadScene(_sceneNameToMove);
    }
}
