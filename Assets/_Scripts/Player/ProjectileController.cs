using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 10f;
    public float lifeTime = 3f; // Mermi 3 saniye sonra kendi kendini yok etsin

    void Start()
    {
        // Mermi sonsuza kadar sahnede kalmasin, 3 saniye sonra yok olsun
        Destroy(gameObject, lifeTime);
    }

    // Mermi bir yere carpinca calisir
    void OnCollisionEnter(Collision collision)
    {
        // Carptigimiz seyde "EnemyHealth" scripti var mi?
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        
        // Eger dusmansa hasar ver
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Mermiyi yok et (Carpisma efektini ilerde buraya ekleriz)
        Destroy(gameObject);
    }
}