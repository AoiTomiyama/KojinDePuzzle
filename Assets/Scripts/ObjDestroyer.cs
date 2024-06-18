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
    /// �F�𔒂ɕς����̂��A�w��b���ҋ@���ď��Ŏ��̏����i�X�R�A�E�^�C�}�[�̏㏸�A�p�[�e�B�N���̐����A�S�̂�List����폜�j���s���B
    /// </summary>
    /// <param name="seconds">�ҋ@����b��</param>
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
    /// �אڂ��Ă���I�u�W�F�N�g�������m���āADestroy�������s�����ǂ�������
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
                //�אڐ���2�ŁA���A�����ꂩ�̗אڂ��Ă���I�u�W�F�N�g�̗אڐ���2�ȏ�̂Ƃ��́A��╡�G�ȏ������K�v�ɂȂ�B
                //�Ⴆ�΁A3���O�p�ɕ���ł���ہA���ꂼ��̗אڐ���2�ƂȂ邪�A�אڑ�����3�Ȃ̂ŏ����Ȃ��悤����������B
                //�������A����ȊO�̃P�[�X�͕K���אڐ���4�ȏ�Ȃ̂Ŕj�󏈗������s����K�v������B

                List<float> initIdMemo = new();
                //�����ŁA�אڂ��Ă���I�u�W�F�N�g�ɌŗLID�̃��X�g���쐬
                foreach (var obj2 in _objList)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    initIdMemo.Add(objScript2._initId);
                }

                foreach (var obj2 in _objList)
                {
                    var objScript2 = obj2.GetComponent<ObjDestroyer>();
                    //�אڂ��Ă���I�u�W�F�N�g���ɁA�אڐ���3�ȏ�i���אڑ����͕K��4�ȏ�j�̂�����Ζ������Ń��[�v��j��
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
                            //�ŗLID�̃��X�g�Ɋ܂܂�Ȃ�ID�����I�u�W�F�N�g�i�܂�A���ꎩ�g�{���X�g��2�ȊO�̃I�u�W�F�N�g�j�����������疳�����Ń��[�v��j��

                            /*
                                ����́A
                                �E�ŏ��ɋL�^����2�ȊO�ŁA�z���\������Ă���i��4�ȏ�ŏz�j���A
                                �E�ŏ��ɋL�^����2�ȊO���A�����Ă���i���ɁA������4���邤���̍�/�E����2�Ԗڂ����ꎩ�g�Ƃ����Ƃ��A�אڂ���2�̊O���ɂ��������̂��킩��j���A
                                �����o���A�j�󏈗������s���Ă���B
                            */

                            if (initIdMemo.Contains(objScript3._initId) != true && objScript3._initId != this._initId)
                            {
                                StartCoroutine(ChangeColorAndDestroy(0.2f));
                                break;
                            }
                        }
                    }
                    
                }
                //����ŁA�I�u�W�F�N�g���m��3�����ŏz��ɗאڂ��Ă����Ԃ��������O���邱�Ƃ��ł����B
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
