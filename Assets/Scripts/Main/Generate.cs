using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Generate : MonoBehaviour
{
    [SerializeField]
    GameObject[] _prefabField;

    GameObject[] _objPrefab = new GameObject[4];

    [SerializeField] 
    GameObject _preview1, _preview2, _comboDisplayer, _scoreDisplayer;

    [SerializeField] 
    Text _scoreText, _comboPrefab, _countUpPrefab, _resultText, _maxComboText, _timerText, _CountDownText;

    [SerializeField]
    Image _circleParameter, _penaltyParameter;

    float _maxCombo = 0, _scoreMemorizer = 0, _initIdGenerate = 0, _penalty = 10, _penaltyInit;
    public static float intervalTime = 0,timer;
    public static int score = 0, combo = 0;
    public List<GameObject> _objList = new();
    GameObject _newObj;
    GameoverDetector _detector;

    bool _hasDetected = false;

    private void Start()
    {
        //static�ϐ��Q���V�[���J�n���ɏ�����
        timer = 400;
        score = 0;
        intervalTime = 0;
        combo = 0;

        //5�F�̂����A4�F��I��
        float r = Random.Range(0, _objPrefab.Length);
        int n = 0;
        for (int i = 0; i < _prefabField.Length; i++)
        {
            if (i != r)
            {
                _objPrefab[n] = _prefabField[i];
                n++;
            }
        }

        //�R���|�[�l���g�̎擾
        _detector = FindObjectOfType<GameoverDetector>();

        //�E�̗\�����ɂ��炩���ߐ���
        _newObj = Instantiate(_objPrefab[Random.Range(0, _objPrefab.Length)], _preview1.transform.position, Quaternion.identity, GameObject.Find("Ball").transform);

        _newObj.GetComponent<ObjDestroyer>()._initId = _initIdGenerate;
        _initIdGenerate++;

        _objList.Add(_newObj);


        _newObj = Instantiate(_objPrefab[Random.Range(0, _objPrefab.Length)], _preview2.transform.position, Quaternion.identity, GameObject.Find("Ball").transform);

        _newObj.GetComponent<ObjDestroyer>()._initId = _initIdGenerate;
        _initIdGenerate++;

        _objList.Add(_newObj);
    }

    /// <summary>
    /// �R���{���i�Ֆʏ�ł����ꂩ�̃I�u�W�F�N�g�����ł����u�Ԃ�1f�̂݁j�ɌĂяo����A�X�R�A�̑����l�E�R���{���̕\���A�l�̋L�^�Ȃǂ��s���B
    /// </summary>
    void Combo()
    {
        //�R���{���𑝂₷
        combo++;

        //�ő�R���{���̋L�^
        if (_maxCombo < combo) _maxCombo = combo;

        //�R���{�����V�[����ɕ\��
        Text showCombo = Instantiate(_comboPrefab, _comboDisplayer.transform);
        showCombo.transform.position = _comboDisplayer.transform.position;
        showCombo.text = combo + " Combo!";

        //�������̃X�R�A���V�[����ɕ\��
        Text text = Instantiate(_countUpPrefab, _scoreDisplayer.transform);
        text.transform.position = _scoreDisplayer.transform.position;
        text.text = "+" + (score - _scoreMemorizer).ToString();

        _scoreMemorizer = score;

        //�������Ԃ��R���{���ɉ����ĉ񕜂�����
        timer += 3 * (combo / 2);
        Debug.Log("Time  +" + 3 * (combo / 2));

        //�X�R�A�̒l���K��10���ɂȂ�悤��0��}��
        var scoreDigit = 10;
        string line = "";
        for (int i = 0; i < scoreDigit - score.ToString().Length; i++)
        {
            line += "0";
        }
        _scoreText.text = "Score: " + line + score;

        //�ő�R���{���ƃX�R�A�����U���g�ɕ\��������
        _maxComboText.text = "<b> Max Combo: " + _maxCombo + " </b>";
        _resultText.text = "<b> Score: " + score + " </b>";

        //�y�i���e�B�[�^�C�}�[�̐ݒ�
        _penalty = _penaltyInit = 8 - score.ToString().Length;

        //�ēx���o�ł���悤�ɂ���
        _hasDetected = false;
    }

    void Update()
    {
        //�C���^�[�o���ƃy�i���e�B�[�^�C�}�[����ʂɉ~�`�Q�[�W�Ƃ��ĕ\��
        _circleParameter.fillAmount = intervalTime / 0.7f;
        _circleParameter.gameObject.transform.parent.gameObject.SetActive(_circleParameter.fillAmount > 0);

        if (intervalTime < 0 )_penalty -= Time.deltaTime;
        _penaltyParameter.fillAmount = _penalty / _penaltyInit;

        //�������Ԃ̐���E�\��
        timer -= Time.deltaTime;
        _timerText.text = "Time: " + Mathf.Round(timer);

        //�������Ԃ�10�b�ȉ��̎��ɁA�J�E���g�_�E����\��
        if (Mathf.Round(timer) <= 10 && Mathf.Round(timer) > 0)
        {
            _CountDownText.text = Mathf.Round(timer).ToString();
            _CountDownText.gameObject.SetActive(true);

            //�J�E���g�_�E���̕\�����A�^�C�}�[�����R���̎��ɑ傫�����ő�A����ȊO�̎��͏��X�ɏk������悤��
            _CountDownText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Round(timer) - timer);
        }
        else
        {
            _CountDownText.gameObject.SetActive(false);
        }


        //�^�C�}�[�������I��15�b�Ɂi�f�o�b�O�p�j
        if (Input.GetKeyDown(KeyCode.T))
        {
            timer = 15;
        }

        if (intervalTime > 0 && intervalTime < 2f)
        {
            intervalTime -= Time.deltaTime;
        }
        else if (intervalTime == 2f)
        {
            intervalTime -= Time.deltaTime;
            Combo();
        }
        else
        {
            combo = 0;
        }

        //�C���^�[�o����0���A�Q�[���I�[�o�[�ɂȂ��ĂȂ��Ƃ��ɁASpace�L�[�������ꂽ�Ƃ����A�y�i���e�B�[�^�C�}�[��0�ɂȂ������ɁA�V���������o���֐������s�B
        //P�L�[�̓f�o�b�O�p�Ȃ̂Ńr���h�i�K�ɂ͖����ɂ��邱��
        if (Input.GetButtonDown("Jump") && intervalTime <= 0  && _detector._isGameOver == false || Input.GetKeyDown(KeyCode.P) || _penalty < 0)
        {
            SpawnNewBall();
        }


        if (_hasDetected == false)
        {
            //�Ֆʏ�ɂ��邷�ׂĂ̋������肵�Ă��鎞�ɁA���ꂼ��̃{�[����DestroyDetection�֐���2�񂾂��Ăяo���B
            float TotalMagnitude = 0;
            foreach (var s in _objList)
            {
                TotalMagnitude += s.GetComponent<Rigidbody2D>().velocity.magnitude;
            }

            if (intervalTime < 0.4f && TotalMagnitude < 0.1f && _detector._isGameOver == false)
            {
                //foreach��DestroyDetection�֐����Ăяo��������2�񂠂�̂́A�אڐ��ɉ����ď������ǂ����̈ꎟ���m�ƁA�אڂ��Ă��铯�F�̃I�u�W�F�N�g��������ہA���g��������񎟌��m�̗������K�v������B
                foreach (var s in _objList)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                foreach (var s in _objList)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                //�J��Ԃ����m�ł��Ȃ��悤��bool�l�Ń��b�N��������B
                _hasDetected = true;
            }
        }
    }

    /// <summary>
    /// �\������������Ֆʏ�Ɉڂ��A�󂢂��X���b�g�ɐV�������𐶐�����B
    /// </summary>
    void SpawnNewBall()
    {
        //�C���^�[�o���̐ݒ�
        intervalTime = 0.7f;
        //�y�i���e�B�[�^�C�}�[�̐ݒ�
        _penalty = _penaltyInit = 8 - score.ToString().Length;


        foreach (GameObject o in _objList)
        {
            if (o.transform.position.x == _preview1.transform.position.x)
            {
                o.transform.position = _preview2.transform.position;
            }
            else if (o.transform.position.x == _preview2.transform.position.x)
            {
                o.transform.position = this.transform.position;
                o.GetComponent<Rigidbody2D>().velocity = Vector2.down * 10;
            }
        }
        _newObj = Instantiate(_objPrefab[Random.Range(0, _objPrefab.Length)], _preview1.transform.position, Quaternion.identity, GameObject.Find("Ball").transform);

        _newObj.GetComponent<ObjDestroyer>()._initId = _initIdGenerate;
        _initIdGenerate++;

        _objList.Add(_newObj);
        _hasDetected = false;
    }
}
