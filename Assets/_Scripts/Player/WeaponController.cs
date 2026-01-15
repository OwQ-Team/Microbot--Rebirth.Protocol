using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform shootingPoint;

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;
    
    [Header("Audio Settings")]
    public AudioSource weaponAudioSource;
    public AudioClip shootSound;
    
    public bool randomizePitch = true;

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = shootingPoint.forward * projectileSpeed;
        }
        
        if (weaponAudioSource != null && shootSound != null)
        {
            if (randomizePitch) 
                weaponAudioSource.pitch = Random.Range(0.9f, 1.1f);
            else 
                weaponAudioSource.pitch = 1f;

            weaponAudioSource.PlayOneShot(shootSound);
        }
    }
}