using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isDead = false;

    [Header("UI References")]
    public Slider healthSlider;
    public Image deathScreenImage;
    public float deathDuration = 2.5f;

    [Header("Damage Flash Settings")]
    public Image damageFlashImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    [Header("Death Effect Settings")]
    public Transform playerCamera;
    public float fallAmount = 1.2f;
    
    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioClip hurtSound;

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

        if (damageFlashImage != null)
        {
            damageFlashImage.color = Color.clear;
        }

        if(playerCamera == null) playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (damageFlashImage != null)
        {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        if (isDead) return;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (playerAudioSource != null && hurtSound != null)
        {
            playerAudioSource.PlayOneShot(hurtSound);
        }
        
        if (damageFlashImage != null)
        {
            damageFlashImage.color = flashColor;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }

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

        PlayerFootsteps sesScripti = GetComponent<PlayerFootsteps>();
        if (sesScripti != null)
        {
            sesScripti.enabled = false;
        }

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