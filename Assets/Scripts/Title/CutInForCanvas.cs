using System.Collections;
using UnityEngine;

public class CutInForCanvas : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CutIn());
    }


    IEnumerator CutIn()
    {
        while (transform.position.x < 3500)
        {
            transform.position += Vector3.right * 80f;
            yield return null;
        }
    }

}
