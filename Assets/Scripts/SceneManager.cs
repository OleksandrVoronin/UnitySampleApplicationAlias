using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script drives the whole scene. On awake it requests to parse and initialize scene,
/// then subscribes to all scene-wide events invoked by InteractibleObjects to respond to them accordingly.
/// InteractableObjects are designed in a way for them to not know about any other objects or the SceneManager itself,
/// making them fully state-agnostic and helping establish a fully top-down control model.
/// </summary>
public class SceneManager : MonoBehaviour
{

    #region Private Properties

    private Camera Camera
    {
        get { return m_Camera = m_Camera ?? Camera.main; }
    }

    private JSONObjectLoader JSONObjectLoader
    {
        get { return m_JSONObjectLoader = m_JSONObjectLoader ?? GetComponent<JSONObjectLoader>(); }
    }

    private SoundManager SoundManager { 
        get { return m_SoundManager = m_SoundManager ?? GetComponentInChildren<SoundManager>(); }
    }

    #endregion


    #region Private Fields

    private Camera m_Camera;

    private List<InteractableObject> m_InteractibleObjects = new List<InteractableObject>();

    private JSONObjectLoader m_JSONObjectLoader;

    private SoundManager m_SoundManager;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        // Parse the scene
        this.JSONObjectLoader.Init();

        // Get all spawned objects
        m_InteractibleObjects.AddRange(GetComponentsInChildren<InteractableObject>());

        // Subscribe to events
        for (int i = 0; i < m_InteractibleObjects.Count; i++)
        {
            HideOtherObjects hideOtherObjects = m_InteractibleObjects[i] as HideOtherObjects;
            if (hideOtherObjects) {
                hideOtherObjects.OnHideOtherObjectsRequested += HideOtherObjects;
            }

            Sound soundEmitter = m_InteractibleObjects[i] as Sound;
            if (soundEmitter)
            {
                soundEmitter.OnSoundTriggered += SoundManager.Play;
            }
        }
    }


    private void Update()
    {
        // On click, raycast and make a hit object, if exists, interact.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = this.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, float.MaxValue))
            {
                InteractableObject interactableObjectHit = hit.collider.gameObject.GetComponent<InteractableObject>();
                if (interactableObjectHit)
                {
                    interactableObjectHit.Interact();
                }
            }
        }
    }

    #endregion


    #region Private Functions

    // Toggles visibility of all objects other than @requestee to @makeVisible
    private void HideOtherObjects(InteractableObject requestee, bool makeVisible)
    {
        for (int i = 0; i < m_InteractibleObjects.Count; i++)
        {
            if (m_InteractibleObjects[i] != requestee)
            {
                VisibilityToggle visibilityToggle = (VisibilityToggle)m_InteractibleObjects[i];

                if (makeVisible)
                {
                    visibilityToggle.Show();
                }
                else
                {
                    visibilityToggle.Hide();
                }
            }
        }
    }

    #endregion
}
