using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム開始前にカウントダウンをするスクリプト。
/// カウントダウンを終えてからボールの配置などを行える。
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

            //カウントダウンの表示が、タイマーが自然数の時に大きさが最大、それ以外の時は徐々に縮小するように
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
