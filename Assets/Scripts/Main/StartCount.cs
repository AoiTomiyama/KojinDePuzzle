using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���J�n�O�ɃJ�E���g�_�E��������X�N���v�g�B
/// �J�E���g�_�E�����I���Ă���{�[���̔z�u�Ȃǂ��s����B
/// </summary>
public class StartCount : MonoBehaviour
{
    private float _startTime = 3;
    Text _startCountText;
    AudioSource _aus;
    [SerializeField]
    AudioClip _countEndSE;

    public float StartTimer { get => _startTime; }

    void Start()
    {
        _startCountText = GetComponent<Text>();
        _startCountText.text = "";
        _aus = GetComponent<AudioSource>();
        StartCoroutine(CountThreeSeconds());
    }

    IEnumerator CountThreeSeconds()
    {
        Debug.Log("Timer Started");
        yield return new WaitForSeconds(0.8f);
        _aus.Play();
        this.gameObject.SetActive(true);
        int timeMemo = Mathf.CeilToInt(StartTimer);
        while (StartTimer > 0)
        {
            yield return null;

            _startCountText.text = Mathf.Ceil(StartTimer).ToString();
            _startCountText.gameObject.SetActive(true);

            if (timeMemo != Mathf.CeilToInt(StartTimer))
            {
                _aus.Play();
            }
            timeMemo = Mathf.CeilToInt(StartTimer);

            //�J�E���g�_�E���̕\�����A�^�C�}�[�����R���̎��ɑ傫�����ő�A����ȊO�̎��͏��X�ɏk������悤��
            _startCountText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Ceil(StartTimer) - StartTimer);

            _startTime -= Time.deltaTime;
        }
        Debug.Log("Timer complete!");
        _aus.PlayOneShot(_countEndSE);
        _startCountText.gameObject.transform.localScale = Vector3.one * 1.3f;
        _startCountText.text = "START!";
        Destroy(this.gameObject, 1);
    }
}
