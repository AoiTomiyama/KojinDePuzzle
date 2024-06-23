using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// ボールの隣接数・破壊処理・色の変化などを行うスクリプト。
/// 消滅時にスコアを加算させる機能も持つ。
/// </summary>
public class ObjDestroyer : MonoBehaviour
{
    [SerializeField]
    GameObject[] particleEmitters;

    private List<GameObject> _objListInAdjacent = new();
    private float _initId = 0;
    SpriteRenderer _sr;
    Color _baseColor;
    Light2D _light;
    AudioSource _aus;
    bool _isCoroutineStarted = false;

    public float InitId { set => _initId = value; }
    public List<GameObject> ObjListInAdjacent { get => _objListInAdjacent; set => _objListInAdjacent = value; }

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseColor = _sr.color;
        _light = _sr.GetComponent<Light2D>();
        _light.color = _baseColor;
        _aus = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && ObjListInAdjacent != null && this.gameObject.name == collision.gameObject.name && collision.isTrigger == false)
        {
            ObjListInAdjacent.Add(collision.gameObject);
            _aus.Play();
            ChangeBrightness();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && ObjListInAdjacent != null && this.gameObject.name == collision.gameObject.name && collision.isTrigger == false)
        {
            ObjListInAdjacent.Remove(collision.gameObject);
            ChangeBrightness();
        }
    }

    /// <summary>
    /// 隣接数に応じて色の明るさ・光源の明るさを変える関数。
    /// </summary>
    void ChangeBrightness()
    {
        float value = 70 * ObjListInAdjacent.Count,
                r = _baseColor.r * 255 + value,
                g = _baseColor.g * 255 + value,
                b = _baseColor.b * 255 + value;
        _sr.color = new Color(r / 255, g / 255, b / 255);

        value = 35 * ObjListInAdjacent.Count;
        r = _baseColor.r * 255 + value;
        g = _baseColor.g * 255 + value;
        b = _baseColor.b * 255 + value;
        _light.color = new Color(r / 255, g / 255, b / 255);
    }

    /// <summary>
    /// 色を白に変えたのち、指定秒数待機して消滅時の処理（スコア・タイマーの上昇、パーティクルの生成、全体のListから削除）を行う。
    /// </summary>
    /// <param name="seconds">待機する秒数</param>
    public IEnumerator ChangeColorAndDestroy(float seconds)
    {
        if (_isCoroutineStarted == false)
        {
            _isCoroutineStarted = true;
            _sr.color = _light.color = Color.white;
            yield return new WaitForSeconds(seconds);

            var particleObject = ((Generate.Combo / 3) < particleEmitters.Length) ? particleEmitters[(Generate.Combo / 3)] : particleEmitters[3];
            Instantiate(particleObject, this.gameObject.transform.position, Quaternion.identity, GameObject.Find("Effect").transform);

            Generate.IntervalTime = 2f;
            Generate.Score += (Generate.Combo > 0) ? 50 * Generate.Combo * Generate.Combo : 50;

            FindObjectOfType<Generate>().ObjListInMainScene.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 隣接しているオブジェクト数を検知して、Destroy処理を行うかどうか処理
    /// </summary>
    public void DestroyDetection()
    {
        foreach (var obj in ObjListInAdjacent)
        {
            var objScript = obj.GetComponent<ObjDestroyer>();
            if (ObjListInAdjacent.Count >= 3)
            {
                StartCoroutine(ChangeColorAndDestroy(0.2f));
                break;
            }
            else if (ObjListInAdjacent.Count == 2 && objScript.ObjListInAdjacent.Count >= 2)
            {
                //隣接数が2で、かつ、いずれかの隣接しているオブジェクトの隣接数が2以上のときは、やや複雑な処理が必要になる。
                //例えば、3つが三角に並んでいる際、それぞれの隣接数は2となるが、隣接総数は3なので消えないよう回避したい。
                //しかし、それ以外のケースは必ず隣接数が4以上なので破壊処理を実行する必要がある。

                List<float> initIdMemo = new();
                //ここで、隣接しているオブジェクトに固有IDのリストを作成
                foreach (var obj2 in ObjListInAdjacent)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    initIdMemo.Add(objScript2._initId);
                }

                foreach (var obj2 in ObjListInAdjacent)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    //隣接しているオブジェクト内に、隣接数が3以上（＝隣接総数は必ず4以上）のがあれば無条件でループを破棄
                    if (objScript2.ObjListInAdjacent.Count > 2)
                    {
                        StartCoroutine(ChangeColorAndDestroy(0.2f));
                        break;
                    }
                    else
                    {
                        foreach (var obj3 in objScript2.ObjListInAdjacent)
                        {
                            var objScript3 = obj3.GetComponent<ObjDestroyer>();
                            //固有IDのリストに含まれないIDを持つオブジェクト（つまり、これ自身＋リストの2つ以外のオブジェクト）が見つかったら無条件でループを破棄

                            /*
                                これは、
                                ・最初に記録した2つ以外で、循環が構成されている（＝4つ以上で循環）か、
                                ・最初に記録した2つ以外が連結している（仮に、横一列に4つあるうちの左/右から2番目がこれ自身としたとき、隣接する2つの外側にもう一つあるのがわかる）か、
                                を検出し、破壊処理を実行している。
                            */

                            if (initIdMemo.Contains(objScript3._initId) != true && objScript3._initId != this._initId)
                            {
                                StartCoroutine(ChangeColorAndDestroy(0.2f));
                                break;
                            }
                        }
                    }

                }
                //これで、オブジェクト同士が3つだけで循環状に隣接している状態だけを除外することができた。
            }
            else if (ObjListInAdjacent.Count == 1 && objScript.ObjListInAdjacent.Count >= 3)
            {
                StartCoroutine(ChangeColorAndDestroy(0.2f));
                break;
            }
            else if (obj.GetComponent<SpriteRenderer>().color == Color.white)
            {
                StartCoroutine(ChangeColorAndDestroy(0.2f));
                break;
            }
        }
    }
}
