using System.Collections;
using UnityEngine;

public class WallReveal : MonoBehaviour
{
    public SpriteRenderer wallRenderer;

    [Header("Reveal Colors")]
    public Color hiddenColor = new Color(0.07f, 0.03f, 0.10f, 1f);    // koyu mor
    public Color revealedColor = new Color(0.55f, 0.25f, 0.85f, 1f);  // parlak mor

    [Header("Outline (optional)")]
    public SpriteRenderer outlineRenderer; // child olarak eklenirse kenar netle�ir
    public Color outlineColor = new Color(1f, 1f, 1f, 0.35f);

    [Header("Timing")]
    public float revealTime = 0.25f;

    Coroutine routine;

    private void Awake()
    {
        if (wallRenderer == null) wallRenderer = GetComponent<SpriteRenderer>();
        ApplyHidden();
    }

    public void Reveal()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(RevealRoutine());
    }

    IEnumerator RevealRoutine()
    {
        // an�nda parlat
        if (wallRenderer != null) wallRenderer.color = revealedColor;
        if (outlineRenderer != null) outlineRenderer.color = outlineColor;

        yield return new WaitForSeconds(revealTime);

        ApplyHidden();
        routine = null;
    }

    private void ApplyHidden()
    {
        if (wallRenderer != null) wallRenderer.color = hiddenColor;
        if (outlineRenderer != null)
        {
            var c = outlineRenderer.color;
            c.a = 0f;
            outlineRenderer.color = c;
        }
    }
}