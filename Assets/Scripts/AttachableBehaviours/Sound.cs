using System;
using UnityEngine;

/// <summary>
/// This script requests a sound to be played
/// </summary>
public class Sound : VisibilityToggle
{
    public event Action OnSoundTriggered;

    public override void Interact()
    {
        if (this.Visible)
        {
            base.Interact();
            OnSoundTriggered?.Invoke();
        }
    }
}
