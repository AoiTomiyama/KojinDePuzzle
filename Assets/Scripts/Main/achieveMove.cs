using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveMove : MonoBehaviour
{
    float theta;
    private void Start()
    {
        StartCoroutine(UpAndDown());
    }

    IEnumerator UpAndDown()
    {
        while (Mathf.Cos(theta) > 0)
        {
            theta += Time.deltaTime;
            transform.position = transform.position + Vector3.up * 0.023f * Mathf.Cos(theta);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        while (Mathf.Cos(theta) < 0)
        {
            theta += Time.deltaTime;
            transform.position = transform.position + Vector3.up * 0.035f * Mathf.Cos(theta);
            yield return null;
        }
        Destroy(gameObject);
    }
}
