using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    [Header("UI References")]
    public Slider healthSlider;
    public Image deathScreenImage;
    public float deathDuration = 2.5f;

    [Header("Damage Flash Settings")]
    public Image damageFlashImage; // Kirmizi vignette resmimiz
    public float flashSpeed = 5f; // Ne kadar hizla sonup gidecek?
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f); // Kirmizi ve yari seffaf

    [Header("Death Effect Settings")]
    public Transform playerCamera;
    public float fallAmount = 1.2f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreenImage != null)
        {
            Color c = deathScreenImage.color;
            c.a = 0f;
            deathScreenImage.color = c;
        }

        // Baslangicta kirmizi ekran varsa sifirlayalim
        if (damageFlashImage != null)
        {
            damageFlashImage.color = Color.clear;
        }

        if(playerCamera == null) playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // --- YENI KISIM: HASAR EFEKTI ---
        // Eger kirmizi resim atanmissa
        if (damageFlashImage != null)
        {
            // Rengi surekli olarak "Color.clear" (tamamen seffaf) olmaya dogru yumusatarak gecir.
            // Biz hasar alinca rengi kirmizi yapacagiz, burasi onu otomatik silecek.
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        // -------------------------------

        if (isDead) return;

        // --- TEST KODU ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        // --- YENI KISIM: HASAR ALINCA KIZART ---
        if (damageFlashImage != null)
        {
            // Resmin rengini aninda belirledigimiz kirmizi tona ayarla.
            // Update fonksiyonu bunu hemen sondurmeye baslayacak.
            damageFlashImage.color = flashColor;
        }
        // --------------------------------------

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }

    // ... Kodun geri kalani (Die, DeathSequence vs.) aynen kaliyor ...
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player is Dead!");

        FirstPersonController hareketScripti = GetComponent<FirstPersonController>();
        if (hareketScripti != null) hareketScripti.enabled = false;

        WeaponController silahScripti = GetComponentInChildren<WeaponController>();
        if (silahScripti != null) silahScripti.enabled = false;

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        float timer = 0f;
        Vector3 startPos = playerCamera.localPosition;
        Quaternion startRot = playerCamera.localRotation;
        Vector3 targetPos = startPos - new Vector3(0, fallAmount, 0);
        Quaternion targetRot = Quaternion.Euler(0, 0, 60);

        while (timer < deathDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / deathDuration;

            if (deathScreenImage != null)
            {
                Color c = deathScreenImage.color;
                c.a = Mathf.Lerp(0f, 1f, progress);
                deathScreenImage.color = c;
            }

            playerCamera.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            playerCamera.localRotation = Quaternion.Slerp(startRot, targetRot, progress);

            yield return null;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}