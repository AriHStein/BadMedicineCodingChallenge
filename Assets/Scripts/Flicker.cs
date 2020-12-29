using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Flicker : MonoBehaviour
{
    [SerializeField] float m_maxFlickImpulse = 10f;
    [SerializeField] float m_maxDragTrailLength = 2f;
    [SerializeField] float m_maxScreenDragDistance = 20f;
    [SerializeField] float m_maxBallSpeedWhileDragging = 0.1f;

    bool m_dragging = false;
    Vector2 m_dragStartPosition;

    bool m_doFlick = false;
    Vector2 m_flickDirection = Vector2.zero;
    float m_flickImuplse = 0f;

    LineRenderer m_lineRenderer;
    Rigidbody2D m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_rb = GetComponent<Rigidbody2D>();

        m_lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        else
        if(m_dragging)
        {
            
            UpdateDrag();

            // I used !GetMouseButton instead of GetMouseButtonUp just incase the mouseUp event gets missed
            // For example if the mouse button is released while over a UI element.
            if(!Input.GetMouseButton(0))
            {
                EndDrag();
            }
        }

        UpdateTrailPreview();
    }

    void StartDrag()
    {
        m_dragging = true;
        m_dragStartPosition = Input.mousePosition;
        m_rb.velocity = m_rb.velocity.normalized * Mathf.Clamp(m_rb.velocity.magnitude, 0, m_maxBallSpeedWhileDragging);
    }

    void UpdateDrag()
    {
        // Convert drag vector from screen space to "flick strength" space
        Vector2 dragVector = (Vector2)Input.mousePosition - m_dragStartPosition;
        m_flickDirection = -dragVector.normalized;
        m_flickImuplse = m_maxFlickImpulse * Ease(Mathf.Clamp01(dragVector.magnitude / m_maxScreenDragDistance));
    }

    void EndDrag()
    {
        // Set a flag to do the flick in the next FixedUpdate
        m_doFlick = true;
        m_dragging = false;
    }

    void UpdateTrailPreview()
    {
        Vector2 trailEndPoint = transform.position - (Vector3)m_flickDirection * m_maxDragTrailLength * m_flickImuplse / m_maxFlickImpulse;
        m_lineRenderer.SetPosition(0, transform.position);
        m_lineRenderer.SetPosition(1, trailEndPoint);
    }

    float Ease(float t)
    {
        // I played with a few different easing functions to see how it changed the feel.
        // Based on formulas here: https://easings.net/

        //return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // EaseInOut
        //return t * t * t; // Ease In
        return 1 - Mathf.Pow(1 - t, 3); // Ease Out
    }

    private void FixedUpdate()
    {
        if(m_doFlick)
        {
            Flick(m_flickDirection, m_flickImuplse);
        }
    }

    void Flick(Vector2 direction, float impulse)
    {
        m_rb.AddForce(direction * impulse, ForceMode2D.Impulse);
        m_doFlick = false;

        m_flickDirection = Vector2.zero;
        m_flickImuplse = 0f;

        m_lineRenderer.SetPosition(1, Vector3.zero);
    }
}
