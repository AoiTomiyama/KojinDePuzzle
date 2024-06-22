using UnityEngine;

/// <summary>
/// �R���{�E�X�R�A�̕\�����ʂ���Ɠ������ׂ̃X�N���v�g
/// </summary>
public class ScoreUp : MonoBehaviour
{
    float theta;
    [SerializeField]
    bool _enableScaling = false;
    private void Start()
    {
        Destroy(gameObject,1.1f);
    }
    void Update()
    {
        theta += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f * Mathf.Cos(3 * theta));
        if (_enableScaling)
        {
            transform.localScale += Vector3.one * 0.004f;
        }
    }
}