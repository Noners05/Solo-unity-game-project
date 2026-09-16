using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f;
    //public float jumphight = 10f;
    
    
    
    PlayerInput playerinput;
    Rigidbody rb;
    Camera playerCam;
    GameObject Currentequipment;

    Vector2 moveInput;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initializing component data
        rb = GetComponent<Rigidbody>();
        playerinput = GetComponent<PlayerInput>();

        Currentequipment = null;

        playerCam = Camera.main;

        //Setting up new move vectors
        moveInput = Vector2.zero;


        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
        
        
        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

       
        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up) +
                            (tempMove.z * transform.forward);





    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void ActivateEquipment()
    {
        if (Currentequipment != null)
        {
            //do something



        }


    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Equipment")
        {
            
            Currentequipment = collision.gameObject;
            collision.gameObject. SetActive(false);


        }



    }





}