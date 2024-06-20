using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image _img;
    Color _color;
    IEnumerator _coroutine;
    void Start()
    {
        _img = GetComponent<Image>();
        _color = _img.color;
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

    private IEnumerator AlphaDecrease()
    {
        while (_color.a >= 0.5)
        {
            yield return null;
            _color.a -= 0.05f;
        }
    }

    private IEnumerator AlphaIncrease()
    {
        while (_color.a <= 1)
        {
            yield return null;
            _color.a += 0.05f;
        }
    }

    private void Update()
    {
        if (_img != null)
        {
            _img.color = _color;
        }
    }
}
