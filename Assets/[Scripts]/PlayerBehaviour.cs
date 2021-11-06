using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;

    private Rigidbody2D rigidbody;

    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        CheckIfGrounded();
    }

    private void Move()
    {
        if(isGrounded)
        {

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            //check sprite flip
            if(x != 0)
                FlipAnimation(x);

            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float horizontalMoveForce = x * horizontalForce;
            float jumpMoveForce = jump * verticalForce;

            float mass = rigidbody.mass * rigidbody.gravityScale;

            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);

            Vector2 velocity = rigidbody.velocity;
            velocity.x *= 0.99f;
            rigidbody.velocity = velocity;
            //

        }
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, float.Epsilon, groundLayerMask);

        isGrounded = (hit) ? true: false;
    }


    private float FlipAnimation(float x)
    {
        x = (x >= 0) ? 1 : -1;
        
        transform.localScale = new Vector3(x, transform.localScale.y);

        return x;
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }

}
