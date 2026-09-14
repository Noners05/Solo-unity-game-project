using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f;
    //public float jumphight = 10f;
    
    
    
    PlayerInput playerinput;
    Rigidbody rb;

    Vector2 moveInput;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initializing component data
        rb = GetComponent<Rigidbody>();
        playerinput = GetComponent<PlayerInput>();

        //Setting up new move vectors
        moveInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed) * transform.right.x * 2;
        tempMove.z = (moveInput.y * speed) * transform.forward.z * 2;

       
        rb.linearVelocity = tempMove;
    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    
    







}