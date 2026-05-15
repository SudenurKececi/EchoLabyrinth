using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        int level = Progression.GetUnlockedLevel();
        SceneManager.LoadScene(level); // Level1=1 ... Level5=5
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetProgress()
    {
        Progression.ResetProgress();
    }
}