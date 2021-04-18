using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NextBallText : MonoBehaviour
{
    [SerializeField] BallSpawner m_spawner = default;

    TextMeshProUGUI m_text;
    const string m_textPrefix = "Next ball in ";

    void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        m_text.text = m_textPrefix + m_spawner.CooldownTimeLeft.ToString();
    }
}
