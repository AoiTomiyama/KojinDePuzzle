using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCount : MonoBehaviour
{
    public float _timer = 3;
    Text _startCountText;
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
        while (_timer > 0)
        {
            yield return null;

            _startCountText.text = Mathf.Ceil(_timer).ToString();
            _startCountText.gameObject.SetActive(true);

            //カウントダウンの表示が、タイマーが自然数の時に大きさが最大、それ以外の時は徐々に縮小するように
            _startCountText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Ceil(_timer) - _timer);

            _timer -= Time.deltaTime;
        }
        Debug.Log("Timer complete!");
        _startCountText.gameObject.transform.localScale = Vector3.one * 1.3f;
        _startCountText.text = "START!";
        Destroy(this.gameObject, 1);
    }
}
