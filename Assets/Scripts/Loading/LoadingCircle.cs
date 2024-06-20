using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCircle : MonoBehaviour
{
    [Header("‰ñ“]‘¬“x")]
    [SerializeField]
    float rotateSpeed = 1;

    float rotateZ;

    private void Start()
    {
        Invoke(nameof(LoadScene), 2);
    }
    void Update()
    {
        rotateZ += rotateSpeed;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);
    }
    void LoadScene()
    {
        SceneManager.LoadScene(2);
    }
}
