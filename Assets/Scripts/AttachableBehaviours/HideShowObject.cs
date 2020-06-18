using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script toggles object's visibility
/// </summary>
public class HideShowObject : VisibilityToggle
{
    public override void Interact()
    {
        base.Interact();
        ToggleVisibility();
    }
}
