using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Rules")]
    public int requiredKeys = 3;

    [Header("UI (optional)")]
    public TextMeshProUGUI itemsText;

    [Header("Exit Door")]
    public ExitDoor exitDoor;

    public int CollectedKeys { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CollectedKeys = 0;
        UpdateUI();

        if (exitDoor != null)
            exitDoor.SetLocked(requiredKeys > 0);
    }

    public void CollectKey()
    {
        CollectedKeys++;
        UpdateUI();

        if (CollectedKeys >= requiredKeys)
        {
            if (exitDoor != null)
                exitDoor.SetLocked(false); // turuncuya dönecek
        }
    }

    private void UpdateUI()
    {
        if (itemsText != null)
            itemsText.text = $"Keys: {CollectedKeys}/{requiredKeys}";
    }
}