using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Settings")]
    public float stepInterval = 0.5f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("References")]
    public AudioSource footstepSource;
    public CharacterController characterController;

    [Header("Audio Files")]
    public AudioClip[] footstepClips; 

    private float nextStepTime = 0f;

    void Start()
    {
        if (footstepSource == null) 
            footstepSource = GetComponent<AudioSource>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckFootsteps();
    }

    void CheckFootsteps()
    {
        if (characterController.isGrounded && characterController.velocity.sqrMagnitude > 2f)
        {
            if (Time.time >= nextStepTime)
            {
                PlayRandomFootstep();
                nextStepTime = Time.time + stepInterval;
            }
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);

        footstepSource.pitch = Random.Range(minPitch, maxPitch);

        footstepSource.PlayOneShot(footstepClips[index]);
    }
}