using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalForce;
    public float verticalForce;
    [Range(0.1f, 0.99f)]
    public float airControlFactor = 0.5f;
    public bool isGrounded;

    private Rigidbody2D rigidbody;
    private Animator animator;

    public PlayerAnimationStates state;

    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        CheckIfGrounded();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            FlipAnimation(x);
        }

        if (isGrounded)
        {

            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            //set animation state based on x input
            state = (x == 0) ? PlayerAnimationStates.Idle : PlayerAnimationStates.Running;
          

            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float horizontalMoveForce = x * horizontalForce;
            float jumpMoveForce = jump * verticalForce;

            AddForce(horizontalMoveForce, jumpMoveForce);
        }
        else
        {
            state = PlayerAnimationStates.Jumping;

            float horizontalMoveForce = x * horizontalForce * airControlFactor;
            AddForce(horizontalMoveForce, 0.0f);
        }

        animator.SetInteger("AnimationState", (int)state);
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true: false;
    }


    private float FlipAnimation(float x)
    {
        x = (x >= 0) ? 1 : -1;
        
        transform.localScale = new Vector3(x, transform.localScale.y);

        return x;
    }


    private void AddForce(float xForce, float yForce)
    {
        float mass = rigidbody.mass * rigidbody.gravityScale;

        rigidbody.AddForce(new Vector2(xForce, yForce) * mass);

        Vector2 velocity = rigidbody.velocity;
        velocity.x *= 0.9f;
        velocity.y *= 0.99f;
        rigidbody.velocity = velocity;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }

}
