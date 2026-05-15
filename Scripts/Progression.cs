using UnityEngine;

public static class Progression
{
    private const string KEY = "UnlockedLevel";
    public const int MaxLevel = 5;

    public static int GetUnlockedLevel()
    {
        return Mathf.Clamp(PlayerPrefs.GetInt(KEY, 1), 1, MaxLevel);
    }

    public static void UnlockLevel(int level)
    {
        int current = GetUnlockedLevel();
        if (level > current)
        {
            PlayerPrefs.SetInt(KEY, Mathf.Clamp(level, 1, MaxLevel));
            PlayerPrefs.Save();
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(KEY);
        PlayerPrefs.Save();
    }
}