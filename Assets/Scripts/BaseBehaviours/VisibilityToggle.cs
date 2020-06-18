using System.Collections;
using UnityEngine;

/// <summary>
/// This class contains current visibility state and means to toggle it.
/// </summary>
public class VisibilityToggle : InteractableObject
{
    #region Public Properties

    public bool Visible
    {
        get { return m_Visible; }
    }

    #endregion


    #region Private Properties

    private Material Material
    {
        get
        {
            return m_Material = m_Material ?? GetComponent<Renderer>().material;
        }
    }

    #endregion


    #region Private Fields

    private Material m_Material;

    private bool m_Visible = true;

    #endregion


    #region Public Methods

    public void ToggleVisibility()
    {
        if (this.Visible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Hide()
    {
        StopCoroutine(ChangeVisibility(0, 0));
        StartCoroutine(ChangeVisibility(0, 0.1f));
        m_Visible = false;
    }

    public void Show()
    {
        StopCoroutine(ChangeVisibility(0, 0));
        StartCoroutine(ChangeVisibility(1, 0.1f));
        m_Visible = true;
    }

    #endregion


    #region Coroutines

    private IEnumerator ChangeVisibility(float targetAlpha, float time)
    {
        float progress = 0;
        float startingAlpha = this.Material.color.a;

        while (progress <= time)
        {
            this.Material.color = m_Material.color.WithAlpha(Mathf.Lerp(startingAlpha, targetAlpha, Mathf.Clamp01(progress / time)));

            progress += Time.deltaTime;
            yield return null;
        }

        this.Material.color = m_Material.color.WithAlpha(targetAlpha);

        yield return null;
    }

    #endregion
}
