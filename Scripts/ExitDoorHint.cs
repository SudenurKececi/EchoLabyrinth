using System.Collections;
using UnityEngine;
using TMPro;

public class ExitDoorHint : MonoBehaviour
{
    public TextMeshPro hintText;
    public float showTime = 0.6f;

    public Color lockedColor = new Color(1f, 0.55f, 0.1f, 1f);   // turuncu
    public Color unlockedColor = new Color(0.55f, 1f, 0.75f, 1f); // açık yeşil

    private ExitDoor door;
    private Coroutine routine;

    private void Awake()
    {
        door = GetComponent<ExitDoor>();

        if (hintText == null)
            hintText = GetComponentInChildren<TextMeshPro>();

        HideImmediate();
    }

    public void RevealHint()
    {
        if (hintText == null) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        UpdateText();
        SetAlpha(1f);

        yield return new WaitForSeconds(showTime);

        SetAlpha(0f);
        routine = null;
    }

    private void UpdateText()
    {
        bool locked = (door != null && door.locked);

        int remaining = 0;
        if (LevelManager.Instance != null)
            remaining = Mathf.Max(0, LevelManager.Instance.requiredKeys - LevelManager.Instance.CollectedKeys);

        if (locked)
        {
            hintText.text = $"LOCKED\n{remaining} key needed";
            hintText.color = lockedColor;
        }
        else
        {
            hintText.text = "EXIT";
            hintText.color = unlockedColor;
        }
    }

    private void HideImmediate()
    {
        if (hintText == null) return;
        var c = hintText.color;
        c.a = 0f;
        hintText.color = c;
    }

    private void SetAlpha(float a)
    {
        var c = hintText.color;
        c.a = a;
        hintText.color = c;
    }
}