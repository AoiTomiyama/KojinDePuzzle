using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SinMove : MonoBehaviour
{
    float timer;

    [SerializeField]
    float radius = 3, rotationSpeed = 1;

    [SerializeField]
    LineRenderer lineRenderer;


    Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        transform.position = new Vector2(startPosition.x + Mathf.Cos(rotationSpeed * timer) * radius + radius, startPosition.y + Mathf.Sin(rotationSpeed * timer) * radius);
        lineRenderer.SetPosition(0, -(Vector2)transform.position + Vector2.right * radius);
    }
}
