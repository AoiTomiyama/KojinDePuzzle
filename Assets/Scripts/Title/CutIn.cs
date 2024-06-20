using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutIn : MonoBehaviour
{
    public void OnButtonPressed()
    {
        StartCoroutine(SceneMove());
    }
    public IEnumerator SceneMove()
    {
        FindObjectOfType<Canvas>().enabled = false;
        while (transform.position.x > 0)
        {
            transform.position += Vector3.left * 0.8f;
            yield return null;
        }
        Debug.Log("Insert Scene Loader Here");
        SceneManager.LoadScene(1);
    }
}
