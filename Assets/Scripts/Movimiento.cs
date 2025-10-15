using UnityEngine;

using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb; 
    
   void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();

        rb.linearVelocity = movement * moveSpeed;
    }
}