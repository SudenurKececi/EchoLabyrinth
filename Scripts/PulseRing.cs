using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PulseRing : MonoBehaviour
{
    public float lifetime = 0.9f;
    public float maxRadiusWorld = 6f;
    [Range(0f, 1f)] public float startAlpha = 0.28f;

    public AnimationCurve radiusCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    public LayerMask wallMask;
    public WallFlash flashPrefab;
    public float ringThickness = 0.18f;
    public float flashGrid = 0.25f;
    public int overlapBufferSize = 64;

    private SpriteRenderer sr;
    private float spriteDiameter;
    private Collider2D[] overlaps;
    private HashSet<Vector2Int> flashedPoints = new HashSet<Vector2Int>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        overlaps = new Collider2D[Mathf.Max(8, overlapBufferSize)];

        spriteDiameter = sr.sprite != null ? sr.sprite.bounds.size.x : 1f;
        if (spriteDiameter <= 0.0001f) spriteDiameter = 1f;
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        float t = 0f;
        Vector2 center = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.05f, lifetime);
            float eased = Mathf.Clamp01(t);

            float r01 = radiusCurve.Evaluate(eased);
            float radius = Mathf.Lerp(0f, maxRadiusWorld, r01);

            float diameter = radius * 2f;
            float scale = diameter / spriteDiameter;
            transform.localScale = Vector3.one * scale;

            float a = startAlpha * alphaCurve.Evaluate(eased);
            SetAlpha(a);

            SampleWalls(center, radius);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SampleWalls(Vector2 center, float radius)
    {
        int count = Physics2D.OverlapCircleNonAlloc(center, radius + ringThickness, overlaps, wallMask);

        for (int i = 0; i < count; i++)
        {
            var col = overlaps[i];
            if (col == null) continue;

            Vector2 p = col.ClosestPoint(center);
            float d = Vector2.Distance(center, p);

            if (Mathf.Abs(d - radius) <= ringThickness)
            {
                var hint = col.GetComponent<ExitDoorHint>();
                if (hint != null) hint.RevealHint();
                if (flashPrefab == null) continue;

                Vector2Int key = new Vector2Int(
                    Mathf.RoundToInt(p.x / flashGrid),
                    Mathf.RoundToInt(p.y / flashGrid)
                );

                if (flashedPoints.Contains(key)) continue;
                flashedPoints.Add(key);
                var reveal = col.GetComponent<WallReveal>();
                if (reveal != null) reveal.Reveal();
                Instantiate(flashPrefab, p, Quaternion.identity);
            }
        }
    }

    private void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = Mathf.Clamp01(a);
        sr.color = c;
    }
}