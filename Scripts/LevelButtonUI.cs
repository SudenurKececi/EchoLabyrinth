using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelButtonUI : MonoBehaviour
{
    public int levelNumber = 1;
    public Button button;
    public TextMeshProUGUI label;
    public GameObject lockedOverlay;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
    }

    public void SetLevel(int level)
    {
        levelNumber = level;
        if (label != null) label.text = $"Level {level}";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(LoadLevel);
    }

    public void SetLocked(bool locked)
    {
        if (lockedOverlay != null) lockedOverlay.SetActive(locked);
        if (button != null) button.interactable = !locked;
    }

    private void LoadLevel()
    {
        // MainMenu buildIndex = 0, Level1 buildIndex = 1 şeklinde
        SceneManager.LoadScene(levelNumber);
    }
}