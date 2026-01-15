using UnityEngine;
using TMPro; // Eger ekrana "Cozuluyor %20" yazdirmak istersen lazim olur

public class BombDefuse : MonoBehaviour
{
    [Header("Ayarlar")]
    public float interactionRange = 3f; // Bombaya ne kadar yakin olmaliyiz?
    public float defuseDuration = 5f;   // Kac saniye basili tutmaliyiz?
    
    [Header("Referanslar")]
    public Transform playerTransform;
    public LevelMissionTimer missionTimerScript; // Sayaci durdurmak icin

    [Header("UI")]
    public TextMeshProUGUI defuseFeedbackText; // "Defusing... 1.2"
    public TextMeshProUGUI interactPromptText;
    
    // O anki durumu tutan degiskenler
    private float currentDefuseTimer = 0f;
    private bool isDefused = false;

    void Start()
    {
        // Player'i otomatik bulalim (Ugrasmamak icin)
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
        
        if (defuseFeedbackText != null) defuseFeedbackText.gameObject.SetActive(false);
        if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDefused || playerTransform == null) 
        {
            if (defuseFeedbackText != null) defuseFeedbackText.gameObject.SetActive(false);
            if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
            return;
        }

        // Player ile Bomba arasindaki mesafeyi olc
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionRange)
        {
            // 1. Durum: E tusuna BASIYORSAK (Cozme islemi)
            if (Input.GetKey(KeyCode.E))
            {
                // "Press E" yazisini gizle
                if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);

                // Cozme islemine basla
                DefuseProcess();
            }
            // 2. Durum: Menzildeyiz ama tusa BASMIYORSAK (Bekleme)
            else
            {
                // Sayaci sifirla
                currentDefuseTimer = 0f;

                // "Defusing" yazisini gizle
                if (defuseFeedbackText != null) defuseFeedbackText.gameObject.SetActive(false);

                // "Press E" yazisini GOSTER
                if (interactPromptText != null)
                {
                    interactPromptText.gameObject.SetActive(true);
                    interactPromptText.text = "Press 'E' to Deactivate";
                }
            }
        }
        else
        {
            currentDefuseTimer = 0f;
            
            // Tum yazilari gizle
            if (defuseFeedbackText != null) defuseFeedbackText.gameObject.SetActive(false);
            if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
        }
    }

    void DefuseProcess()
    {
        // Sureyi arttir
        currentDefuseTimer += Time.deltaTime;

        if (defuseFeedbackText != null)
        {
            defuseFeedbackText.gameObject.SetActive(true); // Yaziyi ac
            
            // Format: "DEFUSING... 2.4"
            // "F1" virgulden sonra 1 basamak gosterir
            defuseFeedbackText.text = "DEFUSING... " + currentDefuseTimer.ToString("F1");
        }

        // Eger sure dolduysa
        if (currentDefuseTimer >= defuseDuration)
        {
            CompleteDefuse();
        }
    }

    void CompleteDefuse()
    {
        isDefused = true; // Tekrar tekrar tetiklenmesin
        
        if (defuseFeedbackText != null) defuseFeedbackText.gameObject.SetActive(false);
        if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
        // Sayac scriptindeki "Basarili" fonksiyonunu cagir
        if (missionTimerScript != null)
        {
            missionTimerScript.MissionSuccess();
        }
    }
    
    // Editor ekraninda bombanin menzilini gormek icin kucuk bir cizim
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}