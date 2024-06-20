using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutIn : MonoBehaviour
{

    [Header("シーン開始時に起動するか")]
    [SerializeField]
    bool _activateOnStart = false;

    private void Start()
    {
        if (_activateOnStart)
        StartCoroutine(StartSceneCutIn());
    }
    public void OnButtonPressed()
    {
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        StartCoroutine(SceneMove("Main"));
    }
    public IEnumerator StartSceneCutIn()
    {
        transform.position = Vector3.left * 3;
        while (transform.position.x < 35)
        {
            transform.position += Vector3.right * 0.8f;
            yield return null;
        }
    }

    public IEnumerator SceneMove(string sceneName)
    {
        LoadingCircle._sceneNameToMove = sceneName;
        transform.position = Vector3.right * 25;
        while (transform.position.x > 0)
        {
            transform.position += Vector3.left * 0.8f;
            yield return null;
        }
        Debug.Log(sceneName);
        SceneManager.LoadScene(1);
    }
}
