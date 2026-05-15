using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Audio")]
    public AudioClip hitSfx;
    [Range(0f,1f)] public float hitVolume = 0.9f;

    private Transform target;
    private bool triggered = false;

    private void Start()
    {
        target = pointB;
    }

    private void Update()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
            target = (target == pointA) ? pointB : pointA;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (hitSfx != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(hitSfx, Camera.main.transform.position, hitVolume);

        // Sesi kesmesin diye minicik gecikme
        Invoke(nameof(RestartNow), 0.70f);
    }

    private void RestartNow()
    {
        GameManagement.Instance?.RestartScene();
    }
}