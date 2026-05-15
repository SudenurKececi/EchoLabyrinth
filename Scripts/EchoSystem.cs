using UnityEngine;

public class EchoSystem : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PulseRing pulsePrefab;

    [Header("Controls")]
    public KeyCode pulseKey = KeyCode.Space;

    [Header("Pulse Timing")]
    public float pulseCooldown = 0.20f;

    [Header("Auto Pulse (movement reveal)")]
    public bool enableAutoPulse = true;
    public float autoPulseDistance = 0.75f;
    public float minSpeedForAutoPulse = 0.10f;

    [Header("Audio (SFX only)")]
    public AudioClip pulseSfx;
    [Range(0f, 1f)] public float pulseVolume = 0.75f;

    private float lastPulseTime = -999f;
    private Vector2 lastPulsePos;

    private PlayerController pc;

    // SFX için ayrý AudioSource (baþka seslerle karýþmasýn)
    private AudioSource sfxSource;

    private void Awake()
    {
        if (player == null) player = transform;

        pc = player.GetComponent<PlayerController>();
        lastPulsePos = player.position;

        // SFX child objesi oluþtur
        var sfxObj = new GameObject("SFX_Source");
        sfxObj.transform.SetParent(player);
        sfxObj.transform.localPosition = Vector3.zero;

        sfxSource = sfxObj.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.clip = null;
        sfxSource.spatialBlend = 0f; // 2D
        sfxSource.volume = 1f;
    }

    private void Update()
    {
        // Space ile manuel pulse
        if (Input.GetKeyDown(pulseKey))
        {
            EmitPulse(manual: true);
        }

        // Hareketle otomatik pulse (ses yok)
        if (enableAutoPulse)
        {
            float moved = Vector2.Distance(player.position, lastPulsePos);
            float speed = (pc != null) ? pc.CurrentVelocity.magnitude : 999f;

            if (moved >= autoPulseDistance && speed >= minSpeedForAutoPulse)
            {
                EmitPulse(manual: false);
            }
        }
    }

    private void EmitPulse(bool manual)
    {
        if (Time.time - lastPulseTime < pulseCooldown) return;

        lastPulseTime = Time.time;
        lastPulsePos = player.position;

        if (pulsePrefab != null)
            Instantiate(pulsePrefab, player.position, Quaternion.identity);

        // Pulse sayacý + ses sadece manual (Space)
        if (manual)
        {
            GameManagement.Instance?.RegisterManualPulse();

            if (pulseSfx != null)
            {
                // “devamlý çalýyor” hissini engellemek için güvenli kes
                sfxSource.Stop();
                sfxSource.PlayOneShot(pulseSfx, pulseVolume);
            }
        }
    }
}