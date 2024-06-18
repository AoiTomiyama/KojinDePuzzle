using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphGenerate : MonoBehaviour
{
    LineRenderer Lr;
    EdgeCollider2D Ec;
    float time;

    [Header("波形の大きさ")]
    [SerializeField]
    float Multiply = 1;

    [Header("波形の幅")]
    [SerializeField]
    float Width = 1;

    [SerializeField]
    Slider sliderWidth, sliderMultiply;

    [SerializeField]
    Dropdown dropdown;

    List<Vector2> pointList = new();

    int count;
    enum TriangleFunc
    {
        sin,
        cos,
        tan,
        sqrt
    }

    [Header("使用する三角関数")]
    [SerializeField]
    //TriangleFunc triFunc = TriangleFunc.sin;

    IEnumerator routine;

    // Start is called before the first frame update
    void Start()
    {
        routine = Generate();
        Lr = GetComponent<LineRenderer>();
        Ec = GetComponent<EdgeCollider2D>();
        Lr.SetPosition(0, Vector2.zero);
        pointList.Add(Vector2.zero);
        Lr.positionCount  = 300;
    }

    void SetLineVertex()
    {
        var m = Multiply * sliderMultiply.value;
        var w = Width * sliderWidth.value;
        count++;
        time += 0.1f;
        Vector2 setPos;
        //if (triFunc == TriangleFunc.sin)
        //{
        //    setPos = new Vector2(time, m * Mathf.Sin(w * time));
        //}
        //else if (triFunc == TriangleFunc.cos)
        //{
        //    setPos = new Vector2(time, m * Mathf.Cos(w * time));
        //}
        //else if (triFunc == TriangleFunc.sqrt)
        //{
        //    setPos = new Vector2(time, m * Mathf.Sqrt(w * time));
        //}
        //else
        //{
        //    setPos = new Vector2(time, m * Mathf.Tan(w * time));
        //}
        if (dropdown.value == 0)
        {
            setPos = new Vector2(time, m * Mathf.Sin(w * time));
        }
        else if (dropdown.value == 1)
        {
            setPos = new Vector2(time, m * Mathf.Cos(w * time));
        }
        else if (dropdown.value == 3)
        {
            setPos = new Vector2(time, m * Mathf.Sqrt(w * time));
        }
        else
        {
            setPos = new Vector2(time, m * Mathf.Tan(w * time));
        }
        Lr.SetPosition(count, setPos);
        pointList.Add(setPos);
        Ec.SetPoints(pointList);
    }

    public void OnClick()
    {
        count = 0; time = 0;
        //Ec.enabled = false;
        pointList.Clear();
        Ec.SetPoints(pointList);
        for (int i = 0; i < Lr.positionCount; i++)
        {
            Lr.SetPosition(i, Vector2.zero);
        }
        StopCoroutine(routine);
        routine = null;
        routine = Generate();
        StartCoroutine(routine);
    }

    IEnumerator Generate()
    {
        while (count < Lr.positionCount - 1)
        {
            SetLineVertex();
            yield return null;
        }
        Ec.enabled = true;
        Debug.Log("グラフの生成完了");
    }
}
