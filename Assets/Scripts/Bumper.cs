using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Bumper : MonoBehaviour
{
    [SerializeField] float m_impulse = default;

    Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // The ball object has the player tag.
        if(collision.collider.CompareTag("Player"))
        {
            Vector2 normal = collision.GetContact(0).normal;
            collision.collider.GetComponent<Rigidbody2D>().AddForce(-normal * m_impulse, ForceMode2D.Impulse);
            m_animator.SetTrigger("Bump");
        }
    }
}
