using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    public bool isAttacking = false;
    public bool hazardDamage = false;
    public float hazardCooldown = 3.0f;

    public int health = 5;
    public float speed = 5;
    public float interactDistance = 6;

    CinemachinePositionComposer cineCam;
    Camera playerCam;
    PlayerInput playerInput;
    Rigidbody rb;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObj;

    public float speedActivate = 5f;
    public float speedBoost = 5f;
    public float speedBoostTimer = 8f;

    public bool speedBoostActivated = false;
    public GameObject currentEquipment;

    Ray interactRay;
    RaycastHit interactHit;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCam = Camera.main;
        cineCam = GameObject.Find("CinemachineCamera").GetComponent<CinemachinePositionComposer>();


        moveInput = Vector2.zero;

       
        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);


        weaponSlot = transform.GetChild(0);


    }

    // Camera rotation code made in fixed update to prevent physics desync
    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Die
        if (health <= 0)


        { }
        if (speedBoostActivated)
        {
            if (speedBoostTimer >= speedActivate)
            {
                speed -= speedBoost;
                speedBoostActivated = false;
            }

            speedBoostTimer += Time.deltaTime;
        }

        // Interact Ray update
        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        // Check if interact ray hits an interactable objects.
        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                // Sets reference to interactable object to "pickupObj"
                pickupObj = interactHit.collider.gameObject;
            }
            else
                pickupObj = null;
        }
        else
            pickupObj = null;
        // If no obj hit or non-interactive obj hit, set pickupObj to null

        // Move code
        Vector3 tempMove = rb.linearVelocity;

        // Normalize input vector to 3D worldspace direction
        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        // Normalize move vector to be relative to player's forward facing direction 
        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up) +
                            (tempMove.z * transform.forward);
    }

    // Take in move input values
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void ActivateEquipment()
    {
        if (currentEquipment != null)
        {
            if (currentEquipment.name == "SpeedDrink")
            {
                speed += speedBoost;

                speedBoostActivated = true;

                currentEquipment = null;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Equipment")
        {
            currentEquipment = collision.gameObject;

            collision.gameObject.SetActive(false);
        }
    }

    // If you have a weapon and the weapon isn't reloading, do the thing
    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }

    // Attack action
    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    isAttacking = true;
                else
                    isAttacking = false;
            }
            else if (context.ReadValueAsButton())
                currentWeapon.fire();


    }


    public void Interact(InputAction.CallbackContext context)
    {
       
        if (context.ReadValueAsButton())
        {
            
            if (pickupObj)
            {
               
                if (pickupObj.tag == "Weapon")
                {
                    if (!currentWeapon)
                    {
                        pickupObj.GetComponent<Weapon>().equip(this);
                    }
                }

               

                if (pickupObj.tag == "Ammo")
                {
                    Destroy(pickupObj);
                    if (currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
                    {
                        int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;

                        if (ammoFill < currentWeapon.ammoRefill)
                        {
                            currentWeapon.ammo += ammoFill;
                        }
                        else
                        {
                            currentWeapon.ammo += currentWeapon.ammoRefill;
                        }
                    }
                }
                
            }
            else if (currentWeapon)
                Reload();
            
        }
    }
  

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Ammo")
        {
            if (currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
            {
                int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;

                if (ammoFill < currentWeapon.ammoRefill)
                {
                    currentWeapon.ammo += ammoFill;
                }
                else
                {
                    currentWeapon.ammo += currentWeapon.ammoRefill;
                }

                Destroy(collision.gameObject);
            }
        }
       
        
        if (collision.gameObject.tag == "Hazard")
        {
            health--;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "Hazard")
        {
            if (!hazardDamage)
                StartCoroutine("damageCooldown");
        }
    }

    
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            if (hazardDamage)
            {
                StopCoroutine("damageCooldown");
                hazardDamage = false;
            }
        }
    }

    
    IEnumerator damageCooldown()
    {
        hazardDamage = true;

        yield return new WaitForSeconds(hazardCooldown);

        health--;
        hazardDamage = false;
    }
}
    