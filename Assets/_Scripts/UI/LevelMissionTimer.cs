using UnityEngine;
using TMPro; // TextMeshPro icin gerekli

public class LevelMissionTimer : MonoBehaviour
{
    [Header("Gorev Ayarlari")]
    public float timeLimit = 30f; // Kac saniye suresi var?
    public string missionMessage = "BOMB DETONATION IN: "; // Yazinin basi
    
    public bool isMissionFinished = false;
    public bool isBombDefused = false;
    
    [Header("Referanslar")]
    public TextMeshProUGUI timerText; // UI'daki yazi
    public PlayerHealth playerHealth; // Oyuncuyu oldurmek icin
    
    public AudioSource ambienceSource;

    void Update()
    {
        // Oyun duraklatildiysa veya sure bittiyse sayma
        if (PauseMenu.GameIsPaused || isMissionFinished) return;

        // Oyuncu zaten olduyse sayaci durdur (Bosuna oldurmeye calismasin)
        if (playerHealth != null && playerHealth.isDead) return;

        // Sureyi azalt
        timeLimit -= Time.deltaTime;

        // Ekrana yazdir (Mathf.CeilToInt sureyi yuvarlar, 29.9 yerine 30 gosterir)
        UpdateTimerUI();

        // Sure bitti mi?
        if (timeLimit <= 0)
        {
            TimeIsUp();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Sifirin altina duserse ekranda eksi sayi gorunmesin, 0 gorunsun
            float displayTime = timeLimit < 0 ? 0 : timeLimit;
            
            // "F0" -> Virgulden sonra hic basamak gosterme demek (Tam sayi)
            timerText.text = missionMessage + displayTime.ToString("F0");
            
            // Sure 5 saniyenin altina duserse kirmizi yapip titretebiliriz (Opsiyonel)
            if (timeLimit <= 5f)
            {
                timerText.color = Color.red;
            }
        }
    }

    void TimeIsUp()
    {
        isMissionFinished = true;
        Debug.Log("SURE BITTI! GOREV BASARISIZ.");

        if (playerHealth != null)
        {
            // Oyuncunun canini 99999 hasar vererek sifirliyoruz.
            // Bu sayede PlayerHealth icindeki Die() fonksiyonu calisiyor
            // ve yere dusme animasyonu + sahne yenileme otomatik yapiliyor.
            playerHealth.TakeDamage(99999f);
        }
    }
    
    public void MissionSuccess()
    {
        isMissionFinished = true; // Oyun bitti (Kazandik)
        
        isBombDefused = true;
        
        if (timerText != null)
        {
            timerText.text = "BOMB DEACTIVATED"; // Yaziyi degistir
            timerText.color = Color.green; // Rengi yesil yap
        }
        
        if (ambienceSource != null)
        {
            ambienceSource.Stop(); 
        }
        
        Debug.Log("TEBRIKLER! BOMBA IMHA EDILDI.");
    }
}