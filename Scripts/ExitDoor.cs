using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public bool locked = true;

    [Header("Visual")]
    public SpriteRenderer sr;
    public Color lockedColor = new Color(0.25f, 0.05f, 0.35f, 1f);   // koyu mor
    public Color unlockedColor = new Color(1f, 0.48f, 0f, 1f);        // turuncu

    [Header("Audio")]
    public AudioClip lockedSfx;
    public AudioClip unlockedSfx;
    [Range(0f, 1f)] public float volume = 0.9f;

    private void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

    public void SetLocked(bool value)
    {
        locked = value;
        ApplyColor();

        if (!locked && unlockedSfx != null)
            AudioSource.PlayClipAtPoint(unlockedSfx, Camera.main.transform.position, volume);
    }

    private void ApplyColor()
    {
        if (sr == null) return;
        sr.color = locked ? lockedColor : unlockedColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (locked)
        {
            if (lockedSfx != null)
                AudioSource.PlayClipAtPoint(lockedSfx, Camera.main.transform.position, volume);
            return;
        }

        // ✅ Kapı açıksa kazan
        GameManagement.Instance?.Win();
    }
}