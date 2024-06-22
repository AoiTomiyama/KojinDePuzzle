using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// メイン画面の実質的なGameManager。
/// 球の生成、管理・スコアや制限時間などのステータスの管理、UIへの表示・実績の表示などを行う。
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
    Text _scoreText;
    [SerializeField]
    Text _comboPrefab;
    [SerializeField]
    Text _countUpPrefab;
    [SerializeField]
    Text _resultText;
    [SerializeField]
    Text _maxComboText;
    [SerializeField]
    Text _timerText;
    [SerializeField]
    Text _CountDownText;

    [SerializeField]
    Image _circleParameter;
    [SerializeField]
    Image _penaltyParameter;

    float _maxCombo = 0, _scoreMemorizer = 0, _initIdGenerate = 0, _penalty = 10, _penaltyInit;
    public static float intervalTime = 0, timer;
    public static int score = 0, combo = 0;

    private List<GameObject> _objListInMainScene = new();
    GameObject _newObj;
    GameoverDetector _detector;
    StartCount _startCount;

    bool _hasDetected = false;
    bool[] _hasAchievementShowed = new bool[2];

    public List<GameObject> ObjListInMainScene { get => _objListInMainScene; }

    private void Start()
    {
        //static変数群をシーン開始時に初期化
        timer = 500;
        score = 0;
        intervalTime = 0;
        combo = 0;

        //タイマーをUIにセット
        _timerText.text = "Time: " + Mathf.Round(timer);

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
        _startCount = FindObjectOfType<StartCount>();

        //右の予測欄にあらかじめ生成
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
        //このCombo関数実行前にスコアは既に増加済なので、134行目は（増加後 - 増加前）で増加分を算出し、生成したテキストに代入している。
        Text text = Instantiate(_countUpPrefab, _scoreDisplayer.transform);
        text.transform.position = _scoreDisplayer.transform.position;
        text.text = "+" + (score - _scoreMemorizer).ToString();

        //スコアがいくつ増加したかを検知するため、_scoreMemorizer変数に増加前のスコアを保存している。
        _scoreMemorizer = score;

        //制限時間をコンボ数に応じて回復させる
        timer += 3 * (combo / 2);
        Debug.Log("Time  +" + 3 * (combo / 2));

        //スコアの値が必ず10桁になるように0を挿入し、テキストに表示。
        _scoreText.text = "Score: " + score.ToString("0000000000");

        //最大コンボ数とスコアをリザルトに表示させる。
        _maxComboText.text = "<b> Max Combo: " + _maxCombo.ToString("0000000000") + " </b>";
        _resultText.text = "<b> Score: " + score + " </b>";

        //ペナルティータイマーの設定
        _penalty = _penaltyInit = 8 - score.ToString().Length;

        //再度検出できるようにする
        _hasDetected = false;
    }

    /// <summary>
    /// UIを制御する部分。赤ゲージのペナルティータイマー・緑ゲージのインターバルタイマーの表示と、制限時間の駆動・表示を行う（スコア・コンボの表示はCombo関数を参照。）
    /// </summary>
    void UIAndTimeControl()
    {
        //インターバルとペナルティータイマーを画面に円形ゲージとして表示
        _circleParameter.fillAmount = intervalTime / 0.7f;
        _circleParameter.gameObject.transform.parent.gameObject.SetActive(_circleParameter.fillAmount > 0);

        if (intervalTime < 0) _penalty -= Time.deltaTime;   
        
        _penaltyParameter.fillAmount = _penalty / _penaltyInit;

        //制限時間の制御・表示
        if (_startCount._timer < 0)
        {
            timer -= Time.deltaTime;
            _timerText.text = "Time: " + Mathf.Round(timer);
        }

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
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    timer = 15;
        //}
    }

    /// <summary>
    /// インターバルの制御を行う。
    /// </summary>
    void Interval()
    {
        if (intervalTime > 0 && intervalTime < 2f)
        {
            intervalTime -= Time.deltaTime;
        }
        else if (intervalTime == 2f)
        {
            //連鎖時に一度だけCombo関数を起動させる。
            intervalTime -= Time.deltaTime;
            Combo();
        }
        else
        {
            combo = 0;
        }
    }

    /// <summary>
    /// 条件を満たしたときに、予測欄から一つ球を盤面上に移し、空いたスロットに新しく球を生成する。
    /// </summary>
    void SpawnNewBall()
    {
        //インターバルが0かつ、ゲームオーバーになってないときに、Spaceキーが押されたときか、ペナルティータイマーが0になった時に、新しく球を出す処理を実行。
        //Pキーはデバッグ用なのでビルド段階には無効にすること
        if (_startCount._timer < 0 && Input.GetButtonDown("Jump") && intervalTime <= 0 && _detector.IsGameOver == false /*|| Input.GetKeyDown(KeyCode.P)*/ || _penalty < 0)
        {
            //インターバルの設定
            intervalTime = 0.7f;
            //ペナルティータイマーの設定
            _penalty = _penaltyInit = 8 - score.ToString().Length;


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
    /// 盤面上のボールが安定している際に、それぞれのボールに付随する、隣接数に応じて消すかどうか判断する関数を実行させる。
    /// </summary>
    void ActivateDetectionToAllBall()
    {
        if (_hasDetected == false)
        {
            //盤面上にあるすべての球が安定している時に、それぞれのボールでDestroyDetection関数を2回だけ呼び出す。
            float TotalMagnitude = 0;
            foreach (var s in ObjListInMainScene)
            {
                TotalMagnitude += s.GetComponent<Rigidbody2D>().velocity.magnitude;
            }

            if (intervalTime < 0.4f && TotalMagnitude < 0.1f && _detector.IsGameOver == false)
            {
                //foreachでDestroyDetection関数を呼び出す部分が2回あるのは、隣接数に応じて消すかどうかの一次検知と、隣接している同色のオブジェクトが消える際、自身も消える二次検知の両方が必要だから。
                foreach (var s in ObjListInMainScene)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                foreach (var s in ObjListInMainScene)
                {
                    s.GetComponent<ObjDestroyer>().DestroyDetection();
                }
                //繰り返し検知できないようにbool値でロックをかける。
                _hasDetected = true;
            }
        }
    }
    /// <summary>
    /// スコアとコンボ数に応じて実績を表示する。
    /// </summary>
    void ShowAchievementByScore()
    {
        if (score >= 1000000 && _hasAchievementShowed[0] == false)
        {
            Instantiate(_achievementPanel[0], FindObjectOfType<Canvas>().transform);
            _hasAchievementShowed[0] = true;
        }
        else if (combo >= 20 && _hasAchievementShowed[1] == false)
        {
            Instantiate(_achievementPanel[1], FindObjectOfType<Canvas>().transform);
            _hasAchievementShowed[1] = true;
        }
    }
}
