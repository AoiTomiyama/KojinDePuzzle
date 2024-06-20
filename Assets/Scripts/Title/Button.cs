using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Image _img;
    Text _text;
    Color _imgColor, _textColor;
    IEnumerator _coroutine;
    void Start()
    {
        _img = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
        _imgColor = _img.color;
        _textColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData p)
    {
        Debug.Log("On Mouse Enter");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = AlphaDecrease();
        StartCoroutine(_coroutine);
    }
    public void OnPointerExit(PointerEventData p)
    {
        Debug.Log("On Mouse Exit");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = AlphaIncrease();
        StartCoroutine(_coroutine);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerExit(eventData);
    }


    private IEnumerator AlphaDecrease()
    {
        while (_imgColor.a >= 0.5)
        {
            yield return null;
            _imgColor.a -= 0.05f;
        }
    }

    private IEnumerator AlphaIncrease()
    {
        while (_imgColor.a <= 1)
        {
            yield return null;
            _imgColor.a += 0.05f;
        }
    }

    private void Update()
    {
        if (_img != null && _text != null)
        {
            _textColor.a = _imgColor.a;
            _img.color = _imgColor;
            _text.color = _textColor;
        }
    }
}
