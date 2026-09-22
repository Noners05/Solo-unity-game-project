using System.Collections;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f;
    //public float jumphight = 10f;
    public float interactDistance = 5.0f;

    int Health = 1;
    public bool takeDmg = false;
    public float Hazardcooldown = 3f;


    PlayerInput playerinput;
    Rigidbody rb;
    Camera playerCam;
    GameObject Currentequipment;
    public GameObject pickupObj;

    Vector2 moveInput;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public bool EnergyDactivated = false;
    public float Speedtimer = 0f;
    public float Speedboost = 8f;

    Ray interactRay;
    RaycastHit interactHit;


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

        //weaponSlot = transform.GetChild(0);

        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);

    }

    // Update is called once per frame
    void Update()
    {

        if (Health <= 0)
            //Die





            if (EnergyDactivated)

                if (Speedtimer >= Speedboost)

                    speed -= Speedboost;
        EnergyDactivated = false;
        Speedtimer += Time.deltaTime;


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
                pickupObj = interactHit.collider.gameObject;
        }

        else
            pickupObj = null;




    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void ActivateEquipment()
    {
        if (Currentequipment != null)
        {
            if (Currentequipment.name == "EnergyD")

                speed += Speedboost;

            EnergyDactivated = true;

            Currentequipment = null;


        }


    }


    public void DropEquipment()
    {
        if (Currentequipment != null)


            Currentequipment.SetActive(true);



    }










    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Equipment")
        {

            Currentequipment = collision.gameObject;

            collision.gameObject.SetActive(false);

            collision.gameObject.SetActive(false);


        }


        if (collision.gameObject.tag == "Hazard")
        {
            Health--;
        }



    }
    /*
    IEnumerator damageCooldown()
    {
        takeDmg = true;

      

    }


    private void OnCollisionStay(Collision collision)
    {
        
    }
    */


    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }



    //public void Interact (InputAction.CallbackContext context);










    private void OnCollisionEnter(Collision collision)
    {
        int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;
        if (collision.gameObject.tag == "Ammo")
        {


            if (currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
            {


                if (ammoFill < currentWeapon.ammoReFill)
                {
                    currentWeapon.ammo += ammoFill;

                }
                else
                {
                    currentWeapon.ammo += currentWeapon.ammoReFill;
                }

                Destroy(collision.gameObject);
            }




        }





    }

    // Interact action
    public void Interact(InputAction.CallbackContext context)
    {
        // If our interact button is active at all
        if (context.ReadValueAsButton())
        {
            // And we have a reference to an object with which to interact
            if (pickupObj)
            {
                // Check if it's a weapon and equip it ONLY if we do not already have a weapon
                if (pickupObj.tag == "Weapon")
                {
                    if (!currentWeapon)
                    {
                        pickupObj.GetComponent<Weapon>().equip(this);
                    }
                }


                // If the interact object is an ammo pickup and you want player to interact with ammo to acquire
                // uncomment the if statement below

                if (pickupObj.tag == "Ammo")
                {
                    Destroy(pickupObj);
                    if (currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
                    {
                        int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;

                        if (ammoFill < currentWeapon.ammoReFill)
                        {
                            currentWeapon.ammo += ammoFill;
                        }
                        else
                        {
                            currentWeapon.ammo += currentWeapon.ammoReFill;
                        }
                    }
                }

            }
            else if (currentWeapon)
                Reload();
        }
    }



}