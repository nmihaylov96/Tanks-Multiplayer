using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletController : MonoBehaviour
{

    Rigidbody rigidBody;

    public AudioClip BulletHitAudio;
    public GameObject bulletImpactEffect;

    [HideInInspector]
    public float bulletSpeed = 15f;
    public int damage = 10;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeBullet(Vector3 originalDirection)
    {
        transform.forward = originalDirection;
        rigidBody.velocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);
        Destroy(gameObject);
    }
}
