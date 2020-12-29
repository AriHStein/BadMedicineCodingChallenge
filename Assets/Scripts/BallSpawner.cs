using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_ballPrefab = default;
    [SerializeField] int m_maxBallCount = 3;
    [SerializeField] float m_spawnCooldown = 3f;

    public float CooldownTimeLeft { get; private set; }
    public int RemainingBallCount { get { return m_maxBallCount - m_currentBallCount; } }
    public event System.Action<int> BallSpawnedEvent;
    
    int m_currentBallCount = 0;
    bool m_waitingToSpawnBall;
    GameObject m_mostRecentSpawn;

    private void Awake()
    {
        if(m_ballPrefab == null)
        {
            Debug.LogError("BallSpawner does not have a reference to the ball prefab! Make sure it is set in the inspector.");
            gameObject.SetActive(false);
            return;
        }

        m_waitingToSpawnBall = true;
    }

    private void Update()
    {
        if(!m_waitingToSpawnBall)
        {
            return;
        }
        
        if(CooldownTimeLeft <= 0)
        {
            SpawnBall();
            return;
        }

        CooldownTimeLeft -= Time.deltaTime;
    }

    void SpawnBall()
    {
        // Spawn a new ball in the middle of the spawn point. 
        // Don't start counting down the cooldown until the ball has left the spawn area.
        // We need to cache a reference the most recent spawn to check against in OnTriggerExit2D
        m_mostRecentSpawn = Instantiate(m_ballPrefab, transform.position, Quaternion.identity);
        m_currentBallCount++;
        m_waitingToSpawnBall = false;
        CooldownTimeLeft = 0f;

        BallSpawnedEvent?.Invoke(RemainingBallCount);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the most recently spawned ball has left the spawn area.
        // Once it has, start a cooldown and spawn a new ball when the cooldown finishes.
        
        if(m_currentBallCount >= m_maxBallCount)
        {
            return;
        }
        
        // Just checking the tag could allow this to trigger when a different ball passes through the spawn area,
        // rather than by the new ball leaving the spawn area. Compare to cached reference to the new ball to prevent that.
        if(collision.CompareTag("Player") && 
            (m_mostRecentSpawn == null || collision.gameObject == m_mostRecentSpawn))
        {
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        CooldownTimeLeft = m_spawnCooldown;
        m_waitingToSpawnBall = true;
        m_mostRecentSpawn = null;
    }
}
