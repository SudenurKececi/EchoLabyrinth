using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip pickupSfx;
    [Range(0f, 1f)] public float volume = 0.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        LevelManager.Instance?.CollectKey();

        if (pickupSfx != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(pickupSfx, Camera.main.transform.position, volume);

        Destroy(gameObject);
    }
}