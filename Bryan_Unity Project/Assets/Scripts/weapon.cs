using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    PlayerControler player;

    public GameObject projectile;
    public Transform firepoint;
    public Camera firingDirection;
    
    
    
    
    [header("meta Atrribute")]
    public bool canfire = true;
    public bool reloading = true;
    public int weaponID;
    public string weaponName;
    public bool holdToAttack = true;

    [header("weapon stats")]

    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;
    //public int firemode;
    //public int currentFM;
    public int clip;
    public int clipsize;

    [header("Ammo Stats")]

    public int ammo;
    public int maxAmmo;
    public int ammoRefill;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firepoint = transform.GetChild(0);
        firingDirection = Camera.main;

    }

   

    public void equip(PlayerControler p)
    {
        player = p;
        player.currentWeapon = this;


        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
        transform.SetParent(player.weaponSlot);

    }
   
    
    public void fire()
    {
        if(canfire && !reloading && clip> 0)
        {
            GameObject p = Instantiate(projectile, firepoint.position, firepoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * projVelocity);
            Destroy(p, projLifespan);
            canfire = false;
            clip--;
            StartCoroutine("cooldownFire");

        }
    }


    public void reload()
    {

        if (clip >= clipsize)
            return;

        int reloadCount = clipsize - clip;

       

        if (ammo < reloadCount)
        {
            clip += ammo;
            ammo = 0;
        }

        else
        {
            clip += reloadCount;
            ammo -= reloadCount;
        }



        StartCoroutine("ReloadingCooldown");

    }

    IEnumerator  cooldownFire()
    {
        yield return new WaitForSeconds(rof);

        if (clip > 0)
        {
            canfire = true;
        }

        
    }


    IEnumerator ReloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);
        
        reloading = false;
        canfire = true;

    }

    









}

internal class headerAttribute : Attribute
{
    private string v;

    public headerAttribute(string v)
    {
        this.v = v;
    }
}