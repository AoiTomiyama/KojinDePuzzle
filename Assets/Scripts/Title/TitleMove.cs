using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�C�g������͂ɉ����Đ������A�g����������F�����炩�ɕω�������X�N���v�g�B
/// TextMeshPro�ŊȒP�ɂł��邱�Ƃ����K�V�[Text�����Ŗ��������Ă邾���B
/// Unity�̃C���X�y�N�^�[����S�Ē����ł���̂ł��̒��g�͂�����Ȃ��B
/// </summary>
public class TitleMove : MonoBehaviour
{
    /// <summary>Time.deltaTime * _waveSpeed �𖈃t���[�����Z�����Ă���ϐ��B������O�p�֐��̈����ɗp����B</summary>
    float _theta;
    /// <summary>��ʏ�ɏo���e�L�X�g�Q��ۊǁE�Ǘ�����z��B</summary>
    Text[] _textArray;

    //�ȉ��̓w�b�_�[�ʂ�̋@�\�Ȃ̂ŁAsummary�͏ȗ��B
    [Header("�^�C�g����")]
    [SerializeField]
    string _title = "Title";

    [Header("������Prefab���Z�b�g")]
    [SerializeField]
    GameObject _textPrefab;

    [Header("�����Ɏg�p����t�H���g���Z�b�g")]
    [SerializeField]
    Font _font;

    [Header("�g�̃X�s�[�h")]
    [SerializeField]
    float _waveSpeed = 1;

    [Header("�g�̑傫��")]
    [SerializeField]
    float _waveAmplify = 1;

    [Header("�g�̕p�x")]
    [SerializeField]
    float _waveFrequency = 1;

    [Header("�e�L�X�g���̕�")]
    [SerializeField]
    float _textPadding = -10;

    [Header("�e�L�X�g�̐F")]
    [SerializeField]
    Gradient _gradient;

    [Header("�e�L�X�g�F�̕ω����x")]
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
