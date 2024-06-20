using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowHowTo : MonoBehaviour
{

    [SerializeField]
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
    public void ShowOnClick()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Show());
    }

    public void HideOnClick()
    {
        StartCoroutine(Hide());
    }
    IEnumerator Show()
    {
        _sizeX = -2000;
        _sizeY = -800;
        while (_sizeX < 0)
        {
            _sizeX += 80;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        while (_sizeY < 0)
        {
            _sizeY += 130;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        _howToPanel.SetActive(true);
    }

    IEnumerator Hide()
    {
        _howToPanel.SetActive(false);
        while (_sizeY > -800)
        {
            _sizeY -= 130;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        while (_sizeX > -2000)
        {
            _sizeX -= 80;
            Vector2 sizeDelta = new(_sizeX, _sizeY);
            _rt.sizeDelta = sizeDelta;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}