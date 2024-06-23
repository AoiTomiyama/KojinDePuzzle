using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �Q�[���J�n�O�ɃJ�E���g�_�E��������X�N���v�g�B
/// �J�E���g�_�E�����I���Ă���{�[���̔z�u�Ȃǂ��s����B
/// </summary>
public class StartCount : MonoBehaviour
{
    private float _startTime = 3;
    Text _startCountText;

    public float StartTimer { get => _startTime; }

    void Start()
    {
        _startCountText = GetComponent<Text>();
        _startCountText.text = "";
        StartCoroutine(CountThreeSeconds());
    }

    IEnumerator CountThreeSeconds()
    {
        Debug.Log("Timer Started");
        yield return new WaitForSeconds(0.8f);
        this.gameObject.SetActive(true);
        while (StartTimer > 0)
        {
            yield return null;

            _startCountText.text = Mathf.Ceil(StartTimer).ToString();
            _startCountText.gameObject.SetActive(true);

            //�J�E���g�_�E���̕\�����A�^�C�}�[�����R���̎��ɑ傫�����ő�A����ȊO�̎��͏��X�ɏk������悤��
            _startCountText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Ceil(StartTimer) - StartTimer);

            _startTime -= Time.deltaTime;
        }
        Debug.Log("Timer complete!");
        _startCountText.gameObject.transform.localScale = Vector3.one * 1.3f;
        _startCountText.text = "START!";
        Destroy(this.gameObject, 1);
    }
}
