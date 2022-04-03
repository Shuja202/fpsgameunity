using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunsystem : MonoBehaviour
{
    //shooting
    public float weaponRange = 100f;
    public bool canAutoFire;
    private bool readytoshoot = true;
    bool shooting; 

    //reloading
    bool reloading;
    public float fireRate;
    float reloadingTime;
    public int availableAmmo, magSize, loadedAmmo;

    //canvas
    UIcontroller myuiCanvas;

    //aiming
    public Transform normalPosition, aimingPosition;
    public float aimSpeed, zoomRate = 40;

    //effects
    public GameObject bigSplash, bulletImpact, bullet;
    public GameObject muzzleFlash;
    public float particleffectLifetime = 1f;

    //player
    public Transform playerHead, firingPosition;

    // Start is called before the first frame update
    void Start()
    {
        magSize = 30;
        loadedAmmo = magSize;
        myuiCanvas = FindObjectOfType<UIcontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
        gunManager();
        updateammotext();
    }

    private void updateammotext()
    {
        myuiCanvas.ammoText.SetText(loadedAmmo + "/" + magSize);
        String temp  = availableAmmo.ToString();
        myuiCanvas.fullAmmoText.SetText(temp);
    }

    private void gunManager()
    {
        if (Input.GetKeyDown(KeyCode.R) && loadedAmmo < magSize && !reloading)
        {
            reload();           
        }

        if (Input.GetMouseButton(1))
        {
            transform.position = Vector3.MoveTowards(transform.position, aimingPosition.position, aimSpeed * Time.deltaTime);
         
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, normalPosition.position, aimSpeed * Time.deltaTime);
            
        }

        if (Input.GetMouseButton(1))
        {
            FindObjectOfType<cameramove>().zoomIn(zoomRate);
        }
        else
        { 
            FindObjectOfType<cameramove>().zoomOut();
        }
         
    }

    private void reload()
    {
       if(availableAmmo > 0)
        {
            reloading = true;

            int bulletToAdd = magSize - loadedAmmo;
            if(bulletToAdd < availableAmmo)
            {
                loadedAmmo = magSize;
                availableAmmo -= bulletToAdd;
            }
            else
            {
                loadedAmmo += availableAmmo;
                availableAmmo = 0;
            }
            StartCoroutine(reloadingpause());
        }
    }

    private void shoot()
    {
        if (canAutoFire)
        {
            shooting = Input.GetMouseButton(0);
        }
        else
        {
            shooting = Input.GetMouseButtonDown(0);
        }

        if (shooting && readytoshoot && loadedAmmo > 0 && !reloading)
        {
            readytoshoot = false;

            particleffectLifetime -= Time.deltaTime;    

            RaycastHit hit;
            if (Physics.Raycast(playerHead.position, playerHead.forward, out hit, weaponRange))
            {
                if (Vector3.Distance(playerHead.position, hit.point) > 2f)
                {
                    firingPosition.LookAt(hit.point);
                    if (hit.collider.CompareTag("Shootable floor"))
                    {
                        particleffectLifetime -= Time.deltaTime;
                        Instantiate(bigSplash, hit.point, Quaternion.LookRotation(hit.normal));
                        if (particleffectLifetime <= 0f)
                            Destroy(bigSplash);
                    }
                    else
                    {
                        particleffectLifetime -= Time.deltaTime;
                        Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(bulletImpact);
                    }
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }   
            }
            else
                firingPosition.LookAt(playerHead.position + (playerHead.forward * 50f));

            Instantiate(bullet, firingPosition.position, firingPosition.rotation, firingPosition.transform);
            Instantiate(muzzleFlash, firingPosition.position, firingPosition.rotation, firingPosition.transform);

            loadedAmmo--;

            StartCoroutine(shootingcontrol());
        }
    }
    IEnumerator shootingcontrol()
    {
        yield return new WaitForSeconds(fireRate);
        readytoshoot = true;   
    }
    IEnumerator reloadingpause()
    {
        yield return new WaitForSeconds(reloadingTime);
        reloading = false;
    }
}
