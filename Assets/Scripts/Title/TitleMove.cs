using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleMove : MonoBehaviour
{
    [Header("ƒ^ƒCƒgƒ‹–¼")]
    [SerializeField]
    string title = "Title";
    float[] theta;
    Text[] textArray;
    float initPosY;
    [SerializeField]
    GameObject textPrefab;

    private void Start()
    {
        var charArray = title.ToCharArray();
        float space = transform.position.x;
        foreach (char c in charArray)
        {
            var go = Instantiate(textPrefab, new Vector2(space, transform.position.y), Quaternion.identity, this.transform);
            go.GetComponent<Text>().text = c.ToString();
            space += 160;
        }
        initPosY = transform.position.y;
        textArray = FindObjectsOfType<Text>();
        theta = new float[textArray.Length];
        foreach (Text text in textArray)
        {
            text.AddComponent<TrailRenderer>();
        }

        var line = "";
        for (int i = 0; i < theta.Length; i++)
        {
            theta[i] = textArray[i].transform.position.x / 200;
            line += theta[i] + ",  ";
        }
        Debug.Log(line);
    }
    private void Update()
    {
        var line = "";
        for (int i = 0; i < theta.Length; i++)
        {
            theta[i] += Time.deltaTime;
            textArray[i].transform.position = new Vector2(textArray[i].transform.position.x, initPosY + 100 * Mathf.Sin(theta[i]));
            line += theta[i] + ",  ";
        }
        Debug.Log(line);
    }
}
