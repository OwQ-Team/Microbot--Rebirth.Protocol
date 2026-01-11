using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Hit Effects")]
    public Material flashMaterial; 
    public float flashDuration = 0.1f; 

    private Renderer enemyRenderer;
    private Material[] originalMaterials; 
    private Coroutine flashCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer != null)
        {
            originalMaterials = enemyRenderer.materials;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // --- YENI EKLENEN KISIM BASLANGIC ---
        // Hasar alinca yapay zekaya haber ver
        EnemyLineOfSight ai = GetComponent<EnemyLineOfSight>();
        if (ai != null)
        {
            ai.OnDamageTaken();
        }
        // --- YENI EKLENEN KISIM BITIS ---

        // Efekti oynat
        if (flashMaterial != null && enemyRenderer != null)
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FlashRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRoutine()
    {
        Material[] flashMats = new Material[originalMaterials.Length];
        for (int i = 0; i < flashMats.Length; i++)
        {
            flashMats[i] = flashMaterial;
        }

        enemyRenderer.materials = flashMats;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.materials = originalMaterials;
        flashCoroutine = null;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}