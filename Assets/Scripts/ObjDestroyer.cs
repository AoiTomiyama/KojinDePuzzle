using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ObjDestroyer : MonoBehaviour
{
    [NonSerialized]
    public List<GameObject> _objList = new();

    SpriteRenderer _sr;
    [SerializeField] GameObject[] particleEmitters;

    [NonSerialized]
    public float _initId = 0;
    Color _baseColor;
    Light2D _light;
    bool _isCoroutineStarted = false;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseColor = _sr.color;
        _light = _sr.GetComponent<Light2D>();
        _light.color = _baseColor;
    }

    void ChangeBrightness()
    {
        float value = 70 * _objList.Count,
                r = _baseColor.r * 255 + value,
                g = _baseColor.g * 255 + value,
                b = _baseColor.b * 255 + value;
        _sr.color = new Color(r /255, g / 255, b / 255);

        value = 35 * _objList.Count;
            r = _baseColor.r * 255 + value;
            g = _baseColor.g * 255 + value;
            b = _baseColor.b * 255 + value;
        _light.color = new Color(r / 255, g / 255, b / 255);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && _objList != null && this.gameObject.name == collision.gameObject.name && collision.isTrigger == false)
        {
            _objList.Add(collision.gameObject);

            ChangeBrightness();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && _objList != null && this.gameObject.name == collision.gameObject.name && collision.isTrigger == false)
        {
            _objList.Remove(collision.gameObject);

            ChangeBrightness();
        }
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

            var particleObject = ((Generate.combo / 3) < particleEmitters.Length) ? particleEmitters[(Generate.combo / 3)] : particleEmitters[3];
            Instantiate(particleObject, this.gameObject.transform.position, Quaternion.identity, GameObject.Find("Effect").transform);

            Generate.intervalTime = 1.3f;
            Generate.score += (Generate.combo > 0) ? 50 * Generate.combo * Generate.combo : 50;

            FindObjectOfType<Generate>()._objList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 隣接しているオブジェクト数を検知して、Destroy処理を行うかどうか処理
    /// </summary>
    public void DestroyDetection()
    {
        foreach (var obj in _objList)
        {
            var objScript = obj.GetComponent<ObjDestroyer>();
            if (_objList.Count >= 3)
            {
                StartCoroutine(ChangeColorAndDestroy(0.2f));
                break;
            }
            else if (_objList.Count == 2 && objScript._objList.Count >= 2)
            {
                //隣接数が2で、かつ、いずれかの隣接しているオブジェクトの隣接数が2以上のときは、やや複雑な処理が必要になる。
                //例えば、3つが三角に並んでいる際、それぞれの隣接数は2となるが、隣接総数は3なので消えないよう回避したい。
                //しかし、それ以外のケースは必ず隣接数が4以上なので破壊処理を実行する必要がある。

                List<float> initIdMemo = new();
                //ここで、隣接しているオブジェクトに固有IDのリストを作成
                foreach (var obj2 in _objList)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    initIdMemo.Add(objScript2._initId);
                }

                foreach (var obj2 in _objList)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    //隣接しているオブジェクト内に、隣接数が3以上（＝隣接総数は必ず4以上）のがあれば無条件でループを破棄
                    if (objScript2._objList.Count > 2)
                    {
                        StartCoroutine(ChangeColorAndDestroy(0.2f));
                        break;
                    }
                    else
                    {
                        foreach (var obj3 in objScript2._objList)
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
            else if (_objList.Count == 1 && objScript._objList.Count >= 3)
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
