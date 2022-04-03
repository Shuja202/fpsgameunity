using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletscript : MonoBehaviour
{
    public float speed, bulletLife;

    public Rigidbody myrigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletShoot();
        Bulletdestruction();
    }

    void Bulletdestruction()
    {
        bulletLife -= Time.deltaTime;

        if (bulletLife <= 0)
        {
            DestroyImmediate(gameObject, true);
        }
    }
    void BulletShoot()
    {
        myrigidbody.velocity = transform.forward * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
