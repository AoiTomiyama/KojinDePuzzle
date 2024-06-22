using System.Collections;
using UnityEngine;

/// <summary>
/// ��������̃p�l�������炩�ɕ\���E��\������X�N���v�g
/// 
/// </summary>
public class ShowHowTo : MonoBehaviour
{
    //�p�l���̍ő�T�C�Y��0�Ȃ̂ŁA�p�l�������S�Ɍ����Ȃ��Ȃ�l�i = 0��菭�Ȃ��j���ǐ��ׂ̈ɒ�`���Ă��邾���B
    const int _minPanelSizeX = -2000;
    const int _minPanelSizeY = -800;

    /// <summary>�J���Ƃ��E����Ƃ���x�����̑��x</summary>
    int _showSpeedX = 80;
    /// <summary>�J���Ƃ��E����Ƃ���y�����̑��x</summary>
    int _showSpeedY = 130;
    /// <summary>�p�l���̃T�C�Y</summary>
    float _sizeX, _sizeY;

    [SerializeField]
    GameObject _howToPanel;
    RectTransform _rt;

    private void Start()
    {
        this.gameObject.SetActive(false);
        _howToPanel.SetActive(false);
        _rt = GetComponent<RectTransform>();
    }
    /// <summary>�J���{�^���������ꂽ�Ƃ��ɌĂяo��</summary>
    public void ShowOnClick()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Show());
    }

    /// <summary>����{�^���������ꂽ�Ƃ��ɌĂяo��</summary>
    public void HideOnClick()
    {
        StartCoroutine(Hide());
    }

    /// <summary>�p�l�������炩�ɊJ��</summary>
    IEnumerator Show()
    {
        _sizeX = _minPanelSizeX;
        _sizeY = _minPanelSizeY;
        while (_sizeX < 0)
        {
            _sizeX += _showSpeedX;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        while (_sizeY < 0)
        {
            _sizeY += _showSpeedY;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        _howToPanel.SetActive(true);
    }

    /// <summary>�p�l�������炩�ɕ���</summary>
    IEnumerator Hide()
    {
        _howToPanel.SetActive(false);
        while (_sizeY > _minPanelSizeY)
        {
            _sizeY -= _showSpeedY;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        while (_sizeX > _minPanelSizeX)
        {
            _sizeX -= _showSpeedX;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}