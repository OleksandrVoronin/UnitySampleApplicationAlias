using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for an interactable object with a small response animation effect.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    private AnimationCurve m_PopCurve;

    // To be overriden by child classes, plays a pop animation by default.
    public virtual void Interact()
    {
        StopAllCoroutines();
        StartCoroutine(AnimatePop(1f, 0.45f));
    }

    private void Awake()
    {
        // A small predefined pop animation.
        Keyframe[] keyframes = {
            new Keyframe(0, 1),
            new Keyframe(0.3f, 1.2f),
            new Keyframe(0.7f, 0.9f),
            new Keyframe(1, 1)
        };

        m_PopCurve = new AnimationCurve(keyframes);
    }

    private IEnumerator AnimatePop(float strength, float time)
    {
        float progress = 0;

        this.transform.localScale = Vector3.one;

        while (progress <= time)
        {
            this.transform.localScale = Vector3.one * m_PopCurve.Evaluate(Mathf.Clamp01(progress / time)) * strength;

            progress += Time.deltaTime;
            yield return null;
        }

        this.transform.localScale = Vector3.one;

        yield return null;
    }
}
