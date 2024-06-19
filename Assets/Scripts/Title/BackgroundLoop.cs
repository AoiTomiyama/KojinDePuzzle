using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [Header("‘¬“x")]
    [SerializeField]
    float _speed = 1;

    void FixedUpdate()
    {
        transform.position += _speed * 0.01f * (Vector3.down + Vector3.right);
        if (Mathf.Abs(transform.position.x) > 1.5f) transform.position = Vector3.zero;
    }
}
