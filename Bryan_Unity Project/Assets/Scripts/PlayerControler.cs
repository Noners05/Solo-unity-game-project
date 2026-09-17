using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f;
    //public float jumphight = 10f;
    public float interactDistance = 5.0f;
    
    
    
    PlayerInput playerinput;
    Rigidbody rb;
    Camera playerCam;
    GameObject Currentequipment;
    public GameObject pickupObject;

    Vector2 moveInput;

    public weapon currentWeapon;
    public Transform weaponSlot;

    Ray interactRay;
   

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

        weaponSlot = transform.GetChild(0);
        
        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);
        
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

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
                pickupObject = interactHit.collider.gameObject;
        }

        else
            pickupObject = null;

       


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


    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }


}