using UnityEngine;

public class DamageBox2D : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10;
    public float pushForce = 5f;
    public Vector2 pushDirection = Vector2.up;
    
    [Header("Cooldown")]
    public float damageCooldown = 1f;
    private float lastDamageTime;
    
    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioClip hitSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        
        if (other.CompareTag("Player"))
        {
            Health2 playerHealth = other.GetComponent<Health2>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
                
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
                }
                
                if (hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
                if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
        }
    }
}
