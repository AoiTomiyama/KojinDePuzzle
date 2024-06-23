using UnityEngine;

/// <summary>
/// �R���{�E�X�R�A�̕\�����ʂ���Ɠ������ׂ̃X�N���v�g
/// </summary>
public class ScoreUp : MonoBehaviour
{
    [Header("�T�C�Y�ω��̔{���iEnableScaling�L�����̂݁j")]
    [SerializeField]
    float _scaleMultiply = 0.004f;
    [Header("�ړ�����X�s�[�h")]
    [SerializeField]
    float _moveSpeed = 0.01f;
    float _theta;
    [SerializeField]
    bool _enableScaling = false;
    private void Start()
    {
        Destroy(gameObject,1.1f);
    }
    void Update()
    {
        _theta += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y + _moveSpeed * Mathf.Cos(3 * _theta));
        if (_enableScaling)
        {
            transform.localScale += Vector3.one * _scaleMultiply;
        }
    }
}