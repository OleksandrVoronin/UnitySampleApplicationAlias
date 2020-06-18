using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically destroys an object x seconds after it was spawned
/// </summary>
public class AutoDestructor : MonoBehaviour {

    [SerializeField]
    private float m_LifeTime = 4.5f;

    private float m_TimeCreated;

    void Awake() {
        m_TimeCreated = Time.time;
    }

    void Update() {
        if (m_TimeCreated + m_LifeTime < Time.time) {
            Destroy(gameObject);
        }
    }
}
