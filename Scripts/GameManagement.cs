using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [Header("UI")]
    public GameObject winPanel;
    public TextMeshProUGUI statsText;
    public KeyCode restartKey = KeyCode.R;

    [Header("Audio")]
    public AudioClip winSfx;
    [Range(0f, 1f)] public float winVolume = 0.9f;

    private AudioSource audioSource;
    private bool won = false;

    private float startTime;
    private int manualPulseCount;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.spatialBlend = 0f;
    }

    private void Start()
    {
        startTime = Time.time;
        manualPulseCount = 0;

        if (winPanel != null) winPanel.SetActive(false);
        if (statsText != null) statsText.text = "";

        Time.timeScale = 1f;
    }

    public void RegisterManualPulse()
    {
        if (won) return;
        manualPulseCount++;
    }

    private void Update()
    {
        if (!won) return;

        if (Input.GetKeyDown(restartKey))
            RestartScene();
    }

    // ✅ DIŞARIDAN ÇAĞRILACAK PUBLIC WIN
    public void Win()
    {
        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // Level1=1
        int currentLevelNumber = currentIndex; // çünkü MainMenu=0, Level1=1...
        Progression.UnlockLevel(currentLevelNumber + 1);
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // Level1 = 1
        Progression.UnlockLevel(currentLevel + 1);
        if (won) return;
        won = true;

        // Player hareket + echo durdur
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            var echo = player.GetComponent<EchoSystem>();
            if (echo != null) echo.enabled = false;
        }

        // kalan efektleri temizle
        foreach (var r in FindObjectsOfType<PulseRing>()) Destroy(r.gameObject);
        foreach (var f in FindObjectsOfType<WallFlash>()) Destroy(f.gameObject);

        if (winPanel != null) winPanel.SetActive(true);

        // Süre + pulse yaz
        float elapsed = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        if (statsText != null)
            statsText.text = $"Süre: {minutes:00}:{seconds:00}\nPulse: {manualPulseCount}";

        if (winSfx != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(winSfx, winVolume);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(1);
    }
}
