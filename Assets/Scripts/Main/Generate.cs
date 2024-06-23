using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���C����ʂ̎����I��GameManager�B
/// ���̐����A�Ǘ��E�X�R�A�␧�����ԂȂǂ̃X�e�[�^�X�̊Ǘ��AUI�ւ̕\���E���т̕\���Ȃǂ��s���B
/// </summary>
public class Generate : MonoBehaviour
{
    [SerializeField]
    GameObject[] _prefabField;
    [SerializeField]
    GameObject[] _achievementPanel;

    GameObject[] _objPrefab = new GameObject[4];

    [SerializeField]
    GameObject _preview1;
    [SerializeField]
    GameObject _preview2;
    [SerializeField]
    GameObject _comboDisplayer;
    [SerializeField]
    GameObject _scoreDisplayer;

    [SerializeField]
    TextMeshProUGUI _scoreText;
    [SerializeField]
    TextMeshProUGUI _comboPrefab;
    [SerializeField]
    TextMeshProUGUI _countUpPrefab;
    [SerializeField]
    TextMeshProUGUI _resultText;
    [SerializeField]
    TextMeshProUGUI _maxComboText;
    [SerializeField]
    TextMeshProUGUI _timerText;
    [SerializeField]
    Text _CountDownText;

    [SerializeField]
    Image _circleParameter;
    [SerializeField]
    Image _penaltyParameter;
    [SerializeField]
    AudioClip[] _comboSE;
    [SerializeField]
    AudioClip _dropSE;
    [SerializeField]
    AudioClip _comboGeneralSE;

    float _maxCombo;
    float _scoreMemorizer;
    float _initIdGenerate;
    float _penalty = 10;
    float _penaltyInit;

    private static float _intervalTime;
    private static float _timer;
    private static int _score;
    private static int _combo;

    private List<GameObject> _objListInMainScene = new();
    GameObject _newObj;
    GameoverDetector _detector;
    StartCount _startCount;
    AudioSource _aus;

    bool _hasDetected = false;
    bool[] _hasAchievementShowed = new bool[2];

    public List<GameObject> ObjListInMainScene { get => _objListInMainScene; }
    public static float IntervalTime { set => _intervalTime = value; }
    public static float Timer { get => _timer; }
    public static int Score { get => _score; set => _score = value; }
    public static int Combo { get => _combo; }

    private void Start()
    {
        //static�ϐ��Q���V�[���J�n���ɏ�����
        _timer = 500;
        Score = 0;
        IntervalTime = 0;
        _combo = 0;

        //�^�C�}�[��UI�ɃZ�b�g
        _timerText.text = "Time: " + Mathf.Round(Timer);

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
        _startCount = FindObjectOfType<StartCount>();
        _aus = GameObject.Find("SE").GetComponent<AudioSource>();

        //�E�̗\�����ɂ��炩���ߐ���
        _newObj = Instantiate(_objPrefab[Random.Range(0, _objPrefab.Length)], _preview1.transform.position, Quaternion.identity, GameObject.Find("Ball").transform);

        _newObj.GetComponent<ObjDestroyer>().InitId = _initIdGenerate;
        _initIdGenerate++;

        ObjListInMainScene.Add(_newObj);


        _newObj = Instantiate(_objPrefab[Random.Range(0, _objPrefab.Length)], _preview2.transform.position, Quaternion.identity, GameObject.Find("Ball").transform);

        _newObj.GetComponent<ObjDestroyer>().InitId = _initIdGenerate;
        _initIdGenerate++;

        ObjListInMainScene.Add(_newObj);
    }
    void Update()
    {
        UIAndTimeControl();
        Interval();
        SpawnNewBall();
        ActivateDetectionToAllBall();
        ShowAchievementByScore();
    }

    /// <summary>
    /// �R���{���i�Ֆʏ�ł����ꂩ�̃I�u�W�F�N�g�����ł����u�Ԃ�1f�̂݁j�ɌĂяo����A�X�R�A�̑����l�E�R���{���̕\���A�l�̋L�^�Ȃǂ��s���B
    /// </summary>
    void ComboFunction()
    {
        //�R���{���𑝂₷
        _combo++;

        //�ő�R���{���̋L�^
        if (_maxCombo < Combo) _maxCombo = Combo;

        //�R���{���ɉ�����SE��炷�B
        _aus.PlayOneShot(_comboGeneralSE, 0.8f);
        _aus.pitch = 1 + Mathf.Clamp((float)_combo - 1f, 0, 9) % 3f / 10f;
        _aus.PlayOneShot(_comboSE[Mathf.Clamp((_combo - 1) / 3, 0, _comboSE.Length - 1)], 0.8f);

        //�R���{�����V�[����ɕ\��
        TextMeshProUGUI showCombo = Instantiate(_comboPrefab, _comboDisplayer.transform);
        showCombo.transform.position = _comboDisplayer.transform.position;
        showCombo.text = Combo + " Combo!";

        //�������̃X�R�A���V�[����ɕ\��
        //����Combo�֐����s�O�ɃX�R�A�͊��ɑ����ςȂ̂ŁA134�s�ڂ́i������ - �����O�j�ő��������Z�o���A���������e�L�X�g�ɑ�����Ă���B
        TextMeshProUGUI text = Instantiate(_countUpPrefab, _scoreDisplayer.transform);
        text.transform.position = _scoreDisplayer.transform.position;
        text.text = "+" + (Score - _scoreMemorizer).ToString();

        //�X�R�A���������������������m���邽�߁A_scoreMemorizer�ϐ��ɑ����O�̃X�R�A��ۑ����Ă���B
        _scoreMemorizer = Score;

        //�������Ԃ��R���{���ɉ����ĉ񕜂�����
        _timer += 3 * (Combo / 2);
        Debug.Log("Time  +" + 3 * (Combo / 2));

        //�X�R�A�̒l���K��10���ɂȂ�悤��0��}�����A�e�L�X�g�ɕ\���B
        _scoreText.text = "Score: " + Score.ToString("0000000000");

        //�ő�R���{���ƃX�R�A�����U���g�ɕ\��������B
        _maxComboText.text = "<b> Max Combo: " + _maxCombo + " </b>";
        _resultText.text = "<b> Score: " + Score.ToString("0000000000") + " </b>";

        //�y�i���e�B�[�^�C�}�[�̐ݒ�
        _penalty = _penaltyInit = 8 - Score.ToString().Length;

        //�ēx���o�ł���悤�ɂ���
        _hasDetected = false;
    }

    /// <summary>
    /// UI�𐧌䂷�镔���B�ԃQ�[�W�̃y�i���e�B�[�^�C�}�[�E�΃Q�[�W�̃C���^�[�o���^�C�}�[�̕\���ƁA�������Ԃ̋쓮�E�\�����s���i�X�R�A�E�R���{�̕\����Combo�֐����Q�ƁB�j
    /// </summary>
    void UIAndTimeControl()
    {
        //�C���^�[�o���ƃy�i���e�B�[�^�C�}�[����ʂɉ~�`�Q�[�W�Ƃ��ĕ\��
        _circleParameter.fillAmount = _intervalTime / 0.7f;
        _circleParameter.gameObject.transform.parent.gameObject.SetActive(_circleParameter.fillAmount > 0);

        if (_intervalTime < 0) _penalty -= Time.deltaTime;

        _penaltyParameter.fillAmount = _penalty / _penaltyInit;

        //�������Ԃ̐���E�\��
        if (_startCount.StartTimer < 0)
        {
            _timer -= Time.deltaTime;
            _timerText.text = "Time: " + Mathf.Round(Timer);
        }

        //�������Ԃ�10�b�ȉ��̎��ɁA�J�E���g�_�E����\��
        if (Mathf.Round(Timer) <= 10 && Mathf.Round(Timer) > 0)
        {
            _CountDownText.text = Mathf.Round(Timer).ToString();
            _CountDownText.gameObject.SetActive(true);

            //�J�E���g�_�E���̕\�����A�^�C�}�[�����R���̎��ɑ傫�����ő�A����ȊO�̎��͏��X�ɏk������悤��
            _CountDownText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Round(Timer) - Timer);
        }
        else
        {
            _CountDownText.gameObject.SetActive(false);
        }

        //�^�C�}�[�������I��15�b�Ɂi�f�o�b�O�p�j
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    timer = 15;
        //}
    }

    /// <summary>
    /// �C���^�[�o���̐�����s���B
    /// </summary>
    void Interval()
    {
        if (_intervalTime > 0 && _intervalTime < 2f)
        {
            _intervalTime -= Time.deltaTime;
        }
        else if (_intervalTime == 2f)
        {
            //�A�����Ɉ�x����Combo�֐����N��������B
            _intervalTime -= Time.deltaTime;
            ComboFunction();
        }
        else
        {
            _combo = 0;
        }
    }

    /// <summary>
    /// �����𖞂������Ƃ��ɁA�\������������Ֆʏ�Ɉڂ��A�󂢂��X���b�g�ɐV�������𐶐�����B
    /// </summary>
    void SpawnNewBall()
    {
        //�C���^�[�o����0���A�Q�[���I�[�o�[�ɂȂ��ĂȂ��Ƃ��ɁASpace�L�[�������ꂽ�Ƃ����A�y�i���e�B�[�^�C�}�[��0�ɂȂ������ɁA�V���������o�����������s�B
        //P�L�[�̓f�o�b�O�p�Ȃ̂Ńr���h�i�K�ɂ͖����ɂ��邱��
        if (_startCount.StartTimer < 0 && Input.GetButtonDown("Jump") && _intervalTime <= 0 && _detector.IsGameOver == false /*|| Input.GetKeyDown(KeyCode.P)*/ || _penalty < 0)
        {
            //�C���^�[�o���̐ݒ�
            IntervalTime = 0.7f;
            //�y�i���e�B�[�^�C�}�[�̐ݒ�
            _penalty = _penaltyInit = 8 - Score.ToString().Length;
            //�������̌��ʉ���炷�B
            if (_aus.pitch != 1)
            {
                _aus.pitch = 1;
            }
            _aus.PlayOneShot(_dropSE, 1);


            foreach (GameObject o in ObjListInMainScene)
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

            _newObj.GetComponent<ObjDestroyer>().InitId = _initIdGenerate;
            _initIdGenerate++;

            ObjListInMainScene.Add(_newObj);
            _hasDetected = false;
        }
    }

    /// <summary>
    /// �Ֆʏ�̃{�[�������肵�Ă���ۂɁA���ꂼ��̃{�[���ɕt������A�אڐ��ɉ����ď������ǂ������f����֐������s������B
    /// </summary>
    void ActivateDetectionToAllBall()
    {
        if (_hasDetected == false)
        {
            //�Ֆʏ�ɂ��邷�ׂĂ̋������肵�Ă��鎞�ɁA���ꂼ��̃{�[����DestroyDetection�֐���2�񂾂��Ăяo���B
            float TotalMagnitude = 0;
            foreach (var s in ObjListInMainScene)
            {
                TotalMagnitude += s.GetComponent<Rigidbody2D>().velocity.magnitude;
            }

            if (_intervalTime < 0.4f && TotalMagnitude < 0.1f && _detector.IsGameOver == false)
            {
                //foreach��DestroyDetection�֐����Ăяo��������2�񂠂�̂́A�אڐ��ɉ����ď������ǂ����̈ꎟ���m�ƁA�אڂ��Ă��铯�F�̃I�u�W�F�N�g��������ہA���g��������񎟌��m�̗������K�v������B
                foreach (var s in ObjListInMainScene)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                foreach (var s in ObjListInMainScene)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                //�J��Ԃ����m�ł��Ȃ��悤��bool�l�Ń��b�N��������B
                _hasDetected = true;
            }
        }
    }
    /// <summary>
    /// �X�R�A�ƃR���{���ɉ����Ď��т�\������B
    /// </summary>
    void ShowAchievementByScore()
    {
        if (Score >= 1000000 && _hasAchievementShowed[0] == false || Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(_achievementPanel[0], FindObjectOfType<Canvas>().transform);
            _hasAchievementShowed[0] = true;
        }
        else if (Combo >= 20 && _hasAchievementShowed[1] == false || Input.GetKeyDown(KeyCode.L))
        {
            Instantiate(_achievementPanel[1], FindObjectOfType<Canvas>().transform);
            _hasAchievementShowed[1] = true;
        }
    }
}
