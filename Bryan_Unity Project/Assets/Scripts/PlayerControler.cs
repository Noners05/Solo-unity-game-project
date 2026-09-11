using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumphight = 10f;

    PlayerInput playerinput;
    Rigidbody rb;

    Vector3 moveInput;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initializing component data
        rb = GetComponent<Rigidbody>();
        playerinput = GetComponent<PlayerInput>();

        //Setting up new move vectors
        moveInput = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempMovement = rb.linearVelocity;

        rb.linearVelocity = (moveInput x * speed) + (moveinput y * speed) + (moveInput z * speed)
    }
}
public void Move(InputAction CallBackContext context)
{
 
}