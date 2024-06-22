using UnityEngine;

/// <summary>
/// カーソルの移動を行うスクリプト。
/// </summary>
public class CursorMove : MonoBehaviour
{
    [SerializeField]
    float _moveDistance = 1.05f;

    int _interval, timer;
    GameoverDetector _detector;

    private void Start()
    {
        _detector = FindObjectOfType<GameoverDetector>();
    }
    void Update()
    {
        if (_detector.IsGameOver == false)
        {
            if (_interval > 0) _interval--;
            var h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                if (_interval == 0)
                {
                    if (h == 1 && transform.position.x < 3.5f)
                    {
                        transform.position = new Vector2(transform.position.x + _moveDistance, transform.position.y);
                    }
                    else if (h == -1 && transform.position.x > -3f)
                    {
                        transform.position = new Vector2(transform.position.x - _moveDistance, transform.position.y);
                    }
                    _interval = (timer > 30) ? 10 : 40;
                }
                timer++;
            }
            else
            {
                _interval = timer = 0;
            }
        }
    }
}
