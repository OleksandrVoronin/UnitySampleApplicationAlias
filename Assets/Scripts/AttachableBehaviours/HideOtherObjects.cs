using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script requests visibility of other objects being toggled
/// </summary>
public class HideOtherObjects : VisibilityToggle
{
    // Visibility toggle event request. Passes requesting object's self and its toggle state
    public event Action<InteractableObject, bool> OnHideOtherObjectsRequested;

    // Current toggle state (make others visibible or invisible)
    private bool m_MakeVisible = false;

    public override void Interact()
    {
        if (this.Visible)
        {
            base.Interact();
            OnHideOtherObjectsRequested?.Invoke(this, m_MakeVisible);
            m_MakeVisible = !m_MakeVisible;
        }
    }
}