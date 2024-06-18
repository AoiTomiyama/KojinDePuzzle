using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField]
    Text _countUpPrefab;

    public void DisplayScore(float scoreUp)
    {
        Text text = Instantiate(_countUpPrefab, transform);
        text.transform.position = transform.position;
        text.text = "+" + scoreUp.ToString();
    }
}
