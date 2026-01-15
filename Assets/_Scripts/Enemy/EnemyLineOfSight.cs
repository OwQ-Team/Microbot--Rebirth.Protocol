using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    [Header("Vision Settings")]
    public float movementSpeed = 4f;
    public float visionRange = 20f;
    [Range(5f, 45f)]
    public float raySpread = 25f;
    
    [Header("Attack Settings")]
    public float attackRange = 2.0f; 
    public float attackDamage = 10f; 
    public float attackRate = 1.5f;  
    private float nextAttackTime = 0f;

    [Header("References")]
    public Transform playerTarget;
    public Transform eyeTransform; 
    public LayerMask detectionLayers; 

    private Animator anim;
    private PlayerHealth playerHealthScript; 
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) 
            {
                playerTarget = p.transform;
            }
        }

        if (playerTarget != null)
        {
            playerHealthScript = playerTarget.GetComponent<PlayerHealth>();
        }

        if (eyeTransform == null) eyeTransform = transform;
    }

    void Update()
    {
        if (playerTarget == null) return;
        
        CheckSightAndLogic();
    }
    
    public void OnDamageTaken()
    {
        TurnToPlayer();
    }

    void TurnToPlayer()
    {
        if (playerTarget == null) return;
        Vector3 targetPostition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        transform.LookAt(targetPostition);
    }

    void CheckSightAndLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        
        bool canSeePlayer = false;
        bool isAttacking = false;

        Vector3 centerDirection = -eyeTransform.up;

        float[] angles = { 0, -raySpread, raySpread };

        foreach (float angle in angles)
        {
            Vector3 rayDirection = Quaternion.AngleAxis(angle, transform.up) * centerDirection;
            Vector3 rayOrigin = eyeTransform.position;

            RaycastHit hit;

            Color debugColor = Color.red;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, visionRange, detectionLayers))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    debugColor = Color.green;
                }
            }

            Debug.DrawRay(rayOrigin, rayDirection * visionRange, debugColor);
        }
        
        if (canSeePlayer)
        {
            TurnToPlayer(); 

            if (distanceToPlayer <= attackRange)
            {
                isAttacking = true; 

                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackRate;
                }
            }
            else
            {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }
        }

        if (anim != null)
        {
            anim.SetBool("isAttacking", isAttacking);
            
            anim.SetBool("isWalking", isAttacking ? false : canSeePlayer);
        }
    }

    void AttackPlayer()
    {
    }

    public void EnemyHitEvent()
    {
        if (playerTarget == null || playerHealthScript == null) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);
        
        if (playerHealthScript.isDead) 
        {
            return;
        }

        if (distance <= attackRange + 0.5f)
        {
            playerHealthScript.TakeDamage(attackDamage);
        }
    }
}