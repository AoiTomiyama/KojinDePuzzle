using UnityEngine;

/// <summary>
/// コンボ・スコアの表示をぬるっと動かす為のスクリプト
/// </summary>
public class ScoreUp : MonoBehaviour
{
    [Header("サイズ変化の倍率（EnableScaling有効時のみ）")]
    [SerializeField]
    float _scaleMultiply = 0.004f;
    [Header("移動するスピード")]
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