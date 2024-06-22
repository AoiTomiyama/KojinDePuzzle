using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UIボタンが押されたとき・カーソルが合った/外れた時の動作。
/// カーソルが合った/外れた時にはぬるっと半透明にする/半透明から戻す。
/// 押されたときは音を鳴らす。
/// 
/// 補足: 通常のGameObjectにカーソルが合ってるかどうかはOnMouseEnterなどで可能だが、UI上では使えない。
/// そこで、IPointerEnterHandler, IPointerExitHandler, IPointerDownHandlerを継承させると、
/// UI上で使えるもの（OnPointerEnterなど）が扱える。
/// </summary>
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Image _img;
    Text _text;
    Color _imgColor, _textColor;
    IEnumerator _coroutine;

    [SerializeField]
    AudioClip _buttonSound;


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
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = AlphaDecrease();
        StartCoroutine(_coroutine);
    }
    public void OnPointerExit(PointerEventData p)
    {
        Debug.Log("On Mouse Exit");

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = AlphaIncrease();
        StartCoroutine(_coroutine);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //押されてすぐボタンが消えるとOnPointerExitの処理がされないままになってしまうので、無理やり実行させている。
        OnPointerExit(eventData);
        if (_buttonSound != null)
        {
            GameObject.Find("SEmanager").GetComponent<AudioSource>().PlayOneShot(_buttonSound);
        }
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
