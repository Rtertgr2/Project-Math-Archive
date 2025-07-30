using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour // ชื่อคลาสต้องตรงกับชื่อไฟล์
{
    public string gameSceneName = "maingame";
    public string settingsSceneName = "SettingsScene";
    public string mainMenuSceneName = "MainMenu";

    // ตรวจสอบว่าฟังก์ชันเป็น public void และไม่มีพารามิเตอร์
    public void StartGame()
    {
        Debug.Log("Start Game button clicked!");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked!");
        SceneManager.LoadScene(settingsSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked!");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu!");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}