using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    public bool isAttacking = false;
    public bool hazardDamage = false;
    public float hazardCooldown = 3.0f;

    public bool EnemyDmg = false;
    public float EnemyCooldown = 2f;

    public int health = 5;
    public float speed = 5;
    public float interactDistance = 6;

    public bool Sprinting = false;
    public float Stamina = 100f;
    public float maxStamina = 100f;
    public float sprintBoost = 2.0f;
    public float sprintCost = 5f;

    public float sprintCooldown = 2;
    public float stamRegenTime = 10;
    public float stamCooldown = 2;
    public bool canSprint = true;
    public bool regenStam = false;
    public bool toggleSprint = true;

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

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCam = Camera.main;
        cineCam = GameObject.Find("CinemachineCamera").GetComponent<CinemachinePositionComposer>();


        moveInput = Vector2.zero;


        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);


        weaponSlot = playerCam.transform.GetChild(0);


    }

    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
    }

    void Update()
    {
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

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
            else
                pickupObj = null;
        }
        else
            pickupObj = null;

        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        if (Sprinting)
        {
            if(moveInput.y == 1 && Stamina > 0)
            {
                tempMove.z *= sprintBoost;

                Stamina -= sprintCost * Time.deltaTime;

                StopCoroutine("StamCD");

                if(Stamina <= 0)
                {
                    canSprint = false;
                    Sprinting = false;
                    StartCoroutine("SprintCD");
                    StartCoroutine("StamCD");
                }
            }

            else
            {
                if (toggleSprint)
                {
                    Sprinting = false;
                    canSprint = false;

                    StartCoroutine("SprintCD");
                    StartCoroutine("StamCD");
                    
                }
            }
        }

        if(!Sprinting)
        {
            if (regenStam)
                Stamina += stamRegenTime * Time.deltaTime;

            if(Stamina >= maxStamina)
            {
                Stamina = maxStamina;
                regenStam = false;
            }
        }

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up) +
                            (tempMove.z * transform.forward);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            if (toggleSprint)
            {
                Sprinting = !Sprinting;
            }
            else
            {
                if (toggleSprint)

                    Sprinting = true;

                else

                    Sprinting = false;

            }
        }
    }

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

    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
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

        if (gameObject.tag == "Enemy")
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
        yield return new WaitForSeconds(EnemyCooldown);

        health--;
        hazardDamage = false;
        
    }

    IEnumerator SprintCD()
    {
        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
    }

    IEnumerator StamCD()
    {
        yield return new WaitForSeconds(stamCooldown);

        regenStam = true;
    }
}
    