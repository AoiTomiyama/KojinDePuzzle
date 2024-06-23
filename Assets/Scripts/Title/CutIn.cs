using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン開始時とシーン移動時にカットインを挿入するスクリプト。
/// シーン移動する際は必ずここを経由すること。
/// </summary>
public class CutIn : MonoBehaviour
{
    [Header("開始時のX座標")]
    [SerializeField]
    private int _startPosX = 3;
    [Header("停止時のX座標")]
    [SerializeField]
    private int _stopPosX = 35;
    [Header("カットインの移動速度")]
    [SerializeField]
    private float _cutInMoveSpeed = 0.8f;
    [Header("シーン開始時に起動するか")]
    [SerializeField]
    bool _activateOnStart = false;

    private void Start()
    {
        if (_activateOnStart)
            StartCoroutine(ShowView());
    }
    /// <summary>
    /// タイトルのスタートボタンが押されたときの動作。
    /// </summary>
    public void OnButtonPressed()
    {
        GameObject.Find("MainMenu").SetActive(false);
        StartCoroutine(HideViewAndSceneMove("Main"));
    }
    /// <summary>
    /// 画面が隠れている状態からシュッとカットインを取り払う。
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
    /// 画面をシュッと隠すカットインを入れたのち、シーンを移動する。
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
