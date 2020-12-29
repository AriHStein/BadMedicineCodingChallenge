using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RemainingBallsText : MonoBehaviour
{   
    TextMeshProUGUI m_text;
    const string text_prefix = "Remaining Balls: ";
    
    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        BallSpawner spawner = FindObjectOfType<BallSpawner>();
        spawner.BallSpawnedEvent += OnBallSpawned;
        OnBallSpawned(spawner.RemainingBallCount);
    }

    void OnBallSpawned(int remainingBalls)
    {
        m_text.text = text_prefix + remainingBalls.ToString();
    }
}
