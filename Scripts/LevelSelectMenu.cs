using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject buttonPrefab;   // ✅ GameObject yaptık
    public Transform gridParent;
    public int levelCount = 10;

    private void Start()
    {
        BuildButtons();
        panel.SetActive(false);
    }

    public void TogglePanel() // ✅ OnClick'te görünmesi için public void şart
    {
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf) RefreshLocks();
    }

    private void BuildButtons()
    {
        // Var olanları temizle (yanlışlıkla iki kere üretirsen)
        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);

        for (int i = 1; i <= levelCount; i++)
        {
            GameObject go = Instantiate(buttonPrefab, gridParent);
            LevelButtonUI ui = go.GetComponent<LevelButtonUI>();
            if (ui != null)
                ui.SetLevel(i);
        }

        RefreshLocks();
    }

    public void RefreshLocks()
    {
        int unlocked = Progression.GetUnlockedLevel();

        foreach (var b in gridParent.GetComponentsInChildren<LevelButtonUI>())
            b.SetLocked(b.levelNumber > unlocked);
    }
}