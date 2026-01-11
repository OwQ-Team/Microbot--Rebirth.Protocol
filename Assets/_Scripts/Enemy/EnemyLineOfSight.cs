using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    [Header("Ayarlar")]
    public float movementSpeed = 4f;
    public float visionRange = 20f;
    
    [Header("Referanslar")]
    public Transform playerTarget;
    public Transform eyeTransform;
    public LayerMask detectionLayers; 

    private Animator anim;
    
    // NOT: isAggressive degiskenini sildik.
    // Artik ozel bir moda girmiyor, sadece yonunu guncelliyor.

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
        }

        if (eyeTransform == null) eyeTransform = transform;
    }

    void Update()
    {
        if (playerTarget == null) return;
        
        // Burada LookAt YOK. Sadece bizi gorurse veya hasar alirsa donecek.
        CheckSightAndMove();
    }
    
    // EnemyHealth scripti tarafindan cagrilan fonksiyon
    public void OnDamageTaken()
    {
        // Hasar aldigi an, player neredeyse oraya don!
        TurnToPlayer();
    }

    // Kod tekrari olmasin diye donme isini ayri fonksiyona koydum
    void TurnToPlayer()
    {
        if (playerTarget == null) return;

        // Sadece Y ekseninde (saga sola) don, yere veya havaya bakma
        Vector3 targetPostition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        transform.LookAt(targetPostition);
    }

    void CheckSightAndMove()
    {
        RaycastHit hit;
        
        Vector3 rayOrigin = eyeTransform.position;
        // SENIN MODEL AYARIN: -Y (Eksi Yesil Ok)
        Vector3 rayDirection = -eyeTransform.up; 

        Color rayColor = Color.red;
        bool goruyorMu = false;

        Debug.DrawRay(rayOrigin, rayDirection * visionRange, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, visionRange, detectionLayers))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Player'i Raycast ile gordugu an:
                
                // 1. Yuzunu Player'a netle (ki hareket edersek takibi birakmasin)
                TurnToPlayer();

                // 2. Ustumuze yuru
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
                
                // 3. Gorsel geri bildirimler
                rayColor = Color.green;
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);
                
                goruyorMu = true;
            }
        }

        // Seni goruyorsa Walk, duvar arkasindaysan Idle
        if (anim != null) anim.SetBool("isWalking", goruyorMu);
    }
}