using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleMove : MonoBehaviour
{
    [Header("ƒ^ƒCƒgƒ‹–¼")]
    [SerializeField]
    string _title = "Title";
    float[] _theta;
    Text[] _textArray;
    float _initPosY;
    [SerializeField]
    GameObject _textPrefab;
    [SerializeField]
    Font _font;

    [SerializeField]
    float _speed = 1, _multiply = 1, _frequency = 1, _padding = -10;
    private void Start()
    {
        var charArray = _title.ToCharArray();
        float space = transform.position.x;
        _textArray = new Text[charArray.Length];
        for (int i = 0; i < charArray.Length; i++)
        {
            var go = Instantiate(_textPrefab, this.transform);
            _textArray[i] = go.GetComponent<Text>();
            _textArray[i].text = charArray[i].ToString();
            _textArray[i].rectTransform.sizeDelta = new Vector2(_textArray[i].preferredWidth, _textArray[i].preferredHeight);

            if (_font != null)
            _textArray[i].font = _font;

            if (i > 0)
            space += _textArray[i].rectTransform.sizeDelta.x / 2 + _textArray[i - 1].rectTransform.sizeDelta.x / 2 + _padding;

            _textArray[i].rectTransform.position = new Vector2(space, transform.position.y);
        }
        _initPosY = transform.position.y;
        _theta = new float[_textArray.Length];
        foreach (Text text in _textArray)
        {
            text.AddComponent<TrailRenderer>();
        }

        var line = "";
        for (int i = 0; i < _theta.Length; i++)
        {
            _theta[i] = _textArray[i].transform.position.x / 200;
            line += _theta[i] + ",  ";
        }
        Debug.Log(line);
    }
    private void FixedUpdate()
    {
        var line = "";
        for (int i = 0; i < _theta.Length; i++)
        {
            _theta[i] += Time.deltaTime * _speed;
            _textArray[i].rectTransform.position = new Vector2(_textArray[i].transform.position.x, _initPosY + 100 * _multiply * Mathf.Sin(_theta[i] * _frequency));
            line += _theta[i] + ",  ";
        }
        Debug.Log(line);
    }
}
