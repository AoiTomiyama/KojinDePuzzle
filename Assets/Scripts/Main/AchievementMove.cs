using System.Collections;
using UnityEngine;

/// <summary>
/// 実績表示の動作を制御する。
/// 
/// ・概要
/// 開始時、一定の高さまで上げる
/// →　1.5秒待機する
/// →　画面外まで移動させ破壊処理。
/// 
/// </summary>
public class AchievementMove : MonoBehaviour
{
    float _theta;

    [Header("表示するときの高さ。 ")]
    [SerializeField]
    const float _showHeight = 0.023f;
    [Header("表示した後の隠すスピード。 ")]
    [SerializeField]
    const float _hideSpeed = 0.035f;
    private void Start()
    {
        StartCoroutine(UpAndDown());
    }
    /// <summary>
    /// 一定の高さまで上げる
    /// →　1.5秒待機する
    /// →　画面外まで移動させ破壊処理。
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
