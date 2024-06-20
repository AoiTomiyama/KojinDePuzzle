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
        //static変数群をシーン開始時に初期化
        timer = 400;
        score = 0;
        intervalTime = 0;
        combo = 0;

        //5色のうち、4色を選択
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

        //コンポーネントの取得
        _detector = FindObjectOfType<GameoverDetector>();

        //右の予測欄にあらかじめ生成
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
    /// コンボ時（盤面上でいずれかのオブジェクトが消滅した瞬間の1fのみ）に呼び出され、スコアの増加値・コンボ数の表示、値の記録などを行う。
    /// </summary>
    void Combo()
    {
        //コンボ数を増やす
        combo++;

        //最大コンボ数の記録
        if (_maxCombo < combo) _maxCombo = combo;

        //コンボ数をシーン上に表示
        Text showCombo = Instantiate(_comboPrefab, _comboDisplayer.transform);
        showCombo.transform.position = _comboDisplayer.transform.position;
        showCombo.text = combo + " Combo!";

        //増加分のスコアをシーン上に表示
        Text text = Instantiate(_countUpPrefab, _scoreDisplayer.transform);
        text.transform.position = _scoreDisplayer.transform.position;
        text.text = "+" + (score - _scoreMemorizer).ToString();

        _scoreMemorizer = score;

        //制限時間をコンボ数に応じて回復させる
        timer += 3 * (combo / 2);
        Debug.Log("Time  +" + 3 * (combo / 2));

        //スコアの値が必ず10桁になるように0を挿入
        var scoreDigit = 10;
        string line = "";
        for (int i = 0; i < scoreDigit - score.ToString().Length; i++)
        {
            line += "0";
        }
        _scoreText.text = "Score: " + line + score;

        //最大コンボ数とスコアをリザルトに表示させ巣
        _maxComboText.text = "<b> Max Combo: " + _maxCombo + " </b>";
        _resultText.text = "<b> Score: " + score + " </b>";

        //ペナルティータイマーの設定
        _penalty = _penaltyInit = 8 - score.ToString().Length;

        //再度検出できるようにする
        _hasDetected = false;
    }

    void Update()
    {
        //インターバルとペナルティータイマーを画面に円形ゲージとして表示
        _circleParameter.fillAmount = intervalTime / 0.7f;
        _circleParameter.gameObject.transform.parent.gameObject.SetActive(_circleParameter.fillAmount > 0);

        if (intervalTime < 0 )_penalty -= Time.deltaTime;
        _penaltyParameter.fillAmount = _penalty / _penaltyInit;

        //制限時間の制御・表示
        timer -= Time.deltaTime;
        _timerText.text = "Time: " + Mathf.Round(timer);

        //制限時間が10秒以下の時に、カウントダウンを表示
        if (Mathf.Round(timer) <= 10 && Mathf.Round(timer) > 0)
        {
            _CountDownText.text = Mathf.Round(timer).ToString();
            _CountDownText.gameObject.SetActive(true);

            //カウントダウンの表示が、タイマーが自然数の時に大きさが最大、それ以外の時は徐々に縮小するように
            _CountDownText.gameObject.transform.localScale = Vector3.one * (1.2f + Mathf.Round(timer) - timer);
        }
        else
        {
            _CountDownText.gameObject.SetActive(false);
        }


        //タイマーを強制的に15秒に（デバッグ用）
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

        //インターバルが0かつ、ゲームオーバーになってないときに、Spaceキーが押されたときか、ペナルティータイマーが0になった時に、新しく球を出す関数を実行。
        //Pキーはデバッグ用なのでビルド段階には無効にすること
        if (Input.GetButtonDown("Jump") && intervalTime <= 0  && _detector._isGameOver == false || Input.GetKeyDown(KeyCode.P) || _penalty < 0)
        {
            SpawnNewBall();
        }


        if (_hasDetected == false)
        {
            //盤面上にあるすべての球が安定している時に、それぞれのボールでDestroyDetection関数を2回だけ呼び出す。
            float TotalMagnitude = 0;
            foreach (var s in _objList)
            {
                TotalMagnitude += s.GetComponent<Rigidbody2D>().velocity.magnitude;
            }

            if (intervalTime < 0.4f && TotalMagnitude < 0.1f && _detector._isGameOver == false)
            {
                //foreachでDestroyDetection関数を呼び出す部分が2回あるのは、隣接数に応じて消すかどうかの一次検知と、隣接している同色のオブジェクトが消える際、自身も消える二次検知の両方が必要だから。
                foreach (var s in _objList)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                foreach (var s in _objList)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                //繰り返し検知できないようにbool値でロックをかける。
                _hasDetected = true;
            }
        }
    }

    /// <summary>
    /// 予測欄から一つ球を盤面上に移し、空いたスロットに新しく球を生成する。
    /// </summary>
    void SpawnNewBall()
    {
        //インターバルの設定
        intervalTime = 0.7f;
        //ペナルティータイマーの設定
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
