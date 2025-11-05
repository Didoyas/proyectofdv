using UnityEngine;

public abstract class EnemigoBase : MonoBehaviour
{
    // Clase base de la que heredarán los enemigos que creemos
    public float speed;
    public int damage;
    public float knockbackForce = 3f;   // Para empujar al player
    public float health = 10f;
    
    protected Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(float amount, Vector2 knockbackDir, float knockbackStrength)
    {
        health -= amount;
        rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}