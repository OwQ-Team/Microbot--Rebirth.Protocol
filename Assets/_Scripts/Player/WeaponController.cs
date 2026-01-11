using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab; // Olusturdugumuz Mermi prefab'i
    public Transform shootingPoint;     // Namlunun ucundaki bos obje

    [Header("Settings")]
    public float projectileSpeed = 20f; // Merminin hizi
    public float fireRate = 0.5f;       // Atis hizi (saniye)

    private float nextFireTime = 0f;

    void Update()
    {
        // Sol tik (Fire1) basildiysa ve atis zamani geldiyse
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // 1. Mermiyi namlu ucunda olustur
        GameObject bullet = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

        // 2. Merminin Rigidbody bilesenini bul
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // 3. Mermiye ileri dogru hiz ver
        if (rb != null)
        {
            // shootingPoint.forward = namlunun baktigi yon
            rb.linearVelocity = shootingPoint.forward * projectileSpeed;
        }
    }
}