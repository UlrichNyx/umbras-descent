using UnityEngine;
using System.Collections;
public class ShadowSuckEffect : MonoBehaviour
{
    private ParticleSystem shadowSuckParticle;
    public bool isSucked;

    public Vector3 playerPosition;
    private ParticleSystem.Particle[] particles;
    public float vanishDistance = 0.01f;
    private Coroutine resetCoroutine;
    void Start()
    {
        // Ensure the particle system is set to emit from a single point
        shadowSuckParticle = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[shadowSuckParticle.main.maxParticles];
        shadowSuckParticle.Stop();
    }

    public void TriggerShadowSuck()
    {
        isSucked = true;
        shadowSuckParticle.Play();

        // Restart the coroutine if it's already running
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        shadowSuckParticle.Play();
        if(gameObject.activeInHierarchy)
        {
            resetCoroutine = StartCoroutine(ResetSuckAfterDelay());
        }
        
    }

    private IEnumerator ResetSuckAfterDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.1f);
        isSucked = false;
        shadowSuckParticle.Stop();
        
    }

    void Update()
    {
        if(isSucked)
        {
            playerPosition = VacuumController.instance.transform.position;
            int numParticlesAlive = shadowSuckParticle.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                // Move particles towards the player
                Vector3 direction = (playerPosition - particles[i].position).normalized;
                particles[i].position += direction * Time.deltaTime * 1.0f; // Adjust speed as needed

                // Check if the particle is close to the player
                if (Vector3.Distance(particles[i].position, playerPosition) < vanishDistance)
                {
                    particles[i].remainingLifetime = 0;
                }
            }

            // Apply the particle changes to the system
            shadowSuckParticle.SetParticles(particles, numParticlesAlive);
        }
    }
}
