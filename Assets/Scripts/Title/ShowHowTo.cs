using System.Collections;
using UnityEngine;

/// <summary>
/// 操作説明のパネルを滑らかに表示・非表示するスクリプト
/// 
/// </summary>
public class ShowHowTo : MonoBehaviour
{
    //パネルの最大サイズが0なので、パネルが完全に見えなくなる値（ = 0より少ない）を可読性の為に定義しているだけ。
    const int _minPanelSizeX = -2000;
    const int _minPanelSizeY = -800;

    /// <summary>開くとき・閉じるときのx方向の速度</summary>
    int _showSpeedX = 80;
    /// <summary>開くとき・閉じるときのy方向の速度</summary>
    int _showSpeedY = 130;
    /// <summary>パネルのサイズ</summary>
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
    /// <summary>開くボタンが押されたときに呼び出す</summary>
    public void ShowOnClick()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Show());
    }

    /// <summary>閉じるボタンが押されたときに呼び出す</summary>
    public void HideOnClick()
    {
        StartCoroutine(Hide());
    }

    /// <summary>パネルを滑らかに開く</summary>
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

    /// <summary>パネルを滑らかに閉じる</summary>
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