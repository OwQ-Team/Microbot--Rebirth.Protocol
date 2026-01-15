using UnityEngine;
using TMPro; // Eger uyari yazisi gostermek istersen

public class ExitDoorTrigger : MonoBehaviour
{
    [Header("References")]
    public LevelMissionTimer missionTimer; // Bombanin durumunu sormak icin
    public VictoryManager victoryManager; // Ekrani acmak icin
    
    [Header("Feedback")]
    // Bomba cozulmeden kapiya gelirse uyari vermek icin (Opsiyonel)
    public TextMeshProUGUI warningText; 

    void OnTriggerEnter(Collider other)
    {
        // Gelen sey Oyuncu mu?
        if (other.CompareTag("Player"))
        {
            // Bomba cozulu mu?
            if (missionTimer.isBombDefused)
            {
                // KAZANDIN!
                victoryManager.ShowVictoryScreen();
            }
            else
            {
                // Bomba hala aktifse oyuncuyu uyar
                Debug.Log("Once bombayi imha etmelisin!");
                
                if (warningText != null)
                {
                    warningText.gameObject.SetActive(true);
                    warningText.text = "DEFUSE THE BOMB FIRST!";
                    Invoke("HideWarning", 3f); // 3 saniye sonra yaziyi sil
                }
            }
        }
    }

    void HideWarning()
    {
        if (warningText != null) warningText.gameObject.SetActive(false);
    }
}