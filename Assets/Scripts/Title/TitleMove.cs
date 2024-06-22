using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルを入力に応じて生成し、波立たせたり色を滑らかに変化させるスクリプト。
/// TextMeshProで簡単にできることをレガシーTextだけで無理矢理やってるだけ。
/// Unityのインスペクターから全て調整できるのでこの中身はいじらない。
/// </summary>
public class TitleMove : MonoBehaviour
{
    /// <summary>Time.deltaTime * _waveSpeed を毎フレーム加算させている変数。これを三角関数の引数に用いる。</summary>
    float _theta;
    /// <summary>画面上に出すテキスト群を保管・管理する配列。</summary>
    Text[] _textArray;

    //以下はヘッダー通りの機能なので、summaryは省略。
    [Header("タイトル名")]
    [SerializeField]
    string _title = "Title";

    [Header("ここにPrefabをセット")]
    [SerializeField]
    GameObject _textPrefab;

    [Header("ここに使用するフォントをセット")]
    [SerializeField]
    Font _font;

    [Header("波のスピード")]
    [SerializeField]
    float _waveSpeed = 1;

    [Header("波の大きさ")]
    [SerializeField]
    float _waveAmplify = 1;

    [Header("波の頻度")]
    [SerializeField]
    float _waveFrequency = 1;

    [Header("テキスト毎の幅")]
    [SerializeField]
    float _textPadding = -10;

    [Header("テキストの色")]
    [SerializeField]
    Gradient _gradient;

    [Header("テキスト色の変化速度")]
    [SerializeField, Range(0.01f, 5)]
    float _colorSpeed = 1;

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
            _textArray[i].color = _gradient.Evaluate((float)i / charArray.Length);

            if (_font != null)
                _textArray[i].font = _font;

            if (i > 0)
                space += _textArray[i].rectTransform.sizeDelta.x / 2 + _textArray[i - 1].rectTransform.sizeDelta.x / 2 + _textPadding;

            _textArray[i].rectTransform.position = new Vector2(space, transform.position.y);
        }
    }
    private void FixedUpdate()
    {
        _theta += Time.deltaTime * _waveSpeed;
        for (int i = 0; i < _textArray.Length; i++)
        {
            _textArray[i].rectTransform.position = new Vector2(_textArray[i].transform.position.x, transform.position.y + 100 * _waveAmplify * Mathf.Sin((_theta + (_textArray[i].rectTransform.position.x / 200)) * _waveFrequency));
            _textArray[i].color = _gradient.Evaluate(((float)i / _textArray.Length + Time.time * _colorSpeed) % 1);
        }
    }
}
