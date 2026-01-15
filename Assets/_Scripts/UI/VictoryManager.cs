using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject victoryUI; // Hazirladigin paneli buraya at

    public void ShowVictoryScreen()
    {
        // 1. Paneli ac
        victoryUI.SetActive(true);

        // 2. Oyunu dondur
        Time.timeScale = 0f;
        
        // 3. Sesleri durdur (Opsiyonel, zafer muzigi calacaksan bunu yapma)
        // AudioListener.pause = true; 

        // 4. Mouse imlecini serbest birak ve goster
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // 5. Arka plandaki Pause menusunun acilmasini engellemek icin
        // PauseMenu.GameIsPaused = true; (Opsiyonel, cakisma olmamasi icin)
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Zamani duzelt
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan cikildi.");
        Application.Quit();
    }
}