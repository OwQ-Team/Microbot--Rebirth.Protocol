using UnityEngine;
using UnityEngine.SceneManagement; // Sahne degisimi icin gerekli kutuphane
using UnityEngine.UI;              // Butonlari tanimak icin gerekli kutuphane

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "GameScene"; // Gececegimiz sahnenin tam adi

    void Start()
    {
        // Butonlara tiklandiginda ne yapacaklarini kod icinden soyluyoruz (Listener)
        
        // Start butonuna tiklaninca StartGame fonksiyonunu calistir
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        // Quit butonuna tiklaninca QuitGame fonksiyonunu calistir
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    // Oyunu baslatan fonksiyon
    private void StartGame()
    {
        Debug.Log("Loading Scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    // Oyundan cikan fonksiyon
    private void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();

        // Unity Editor'de calisirken oyunun kapandigini gormek icin bu kodu ekliyoruz
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}