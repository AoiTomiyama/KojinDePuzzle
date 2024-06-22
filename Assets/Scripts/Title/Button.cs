using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI�{�^���������ꂽ�Ƃ��E�J�[�\����������/�O�ꂽ���̓���B
/// �J�[�\����������/�O�ꂽ���ɂ͂ʂ���Ɣ������ɂ���/����������߂��B
/// �����ꂽ�Ƃ��͉���炷�B
/// 
/// �⑫: �ʏ��GameObject�ɃJ�[�\���������Ă邩�ǂ�����OnMouseEnter�Ȃǂŉ\�����AUI��ł͎g���Ȃ��B
/// �����ŁAIPointerEnterHandler, IPointerExitHandler, IPointerDownHandler���p��������ƁA
/// UI��Ŏg������́iOnPointerEnter�Ȃǁj��������B
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
        //������Ă����{�^�����������OnPointerExit�̏���������Ȃ��܂܂ɂȂ��Ă��܂��̂ŁA���������s�����Ă���B
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
