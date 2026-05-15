using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WallFlash : MonoBehaviour
{
    public float lifetime = 0.22f;
    public float startScale = 0.22f;
    public float endScale = 0.35f;
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private SpriteRenderer sr;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    private void OnEnable()
    {
        transform.localScale = Vector3.one * startScale;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float t = 0f;
        Color baseColor = sr.color;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.05f, lifetime);
            float eased = Mathf.Clamp01(t);

            float a = alphaCurve.Evaluate(eased);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);

            float sc = Mathf.Lerp(startScale, endScale, eased);
            transform.localScale = Vector3.one * sc;

            yield return null;
        }

        Destroy(gameObject);
    }
}