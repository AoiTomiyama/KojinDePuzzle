using System.Collections;
using UnityEngine;

/// <summary>
/// ���ѕ\���̓���𐧌䂷��B
/// 
/// �E�T�v
/// �J�n���A���̍����܂ŏグ��
/// ���@1.5�b�ҋ@����
/// ���@��ʊO�܂ňړ������j�󏈗��B
/// 
/// </summary>
public class AchievementMove : MonoBehaviour
{
    float _theta;

    [Header("�\������Ƃ��̍����B ")]
    [SerializeField]
    const float _showHeight = 0.023f;
    [Header("�\��������̉B���X�s�[�h�B ")]
    [SerializeField]
    const float _hideSpeed = 0.035f;
    private void Start()
    {
        StartCoroutine(UpAndDown());
    }
    /// <summary>
    /// ���̍����܂ŏグ��
    /// ���@1.5�b�ҋ@����
    /// ���@��ʊO�܂ňړ������j�󏈗��B
    /// </summary>
    IEnumerator UpAndDown()
    {
        while (Mathf.Cos(_theta) > 0)
        {
            _theta += Time.deltaTime;
            transform.position += _showHeight * Mathf.Cos(_theta) * Vector3.up;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        while (Mathf.Cos(_theta) < 0)
        {
            _theta += Time.deltaTime;
            transform.position += _hideSpeed * Mathf.Cos(_theta) * Vector3.up;
            yield return null;
        }
        Destroy(gameObject);
    }
}
