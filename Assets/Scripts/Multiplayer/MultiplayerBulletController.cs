using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class MultiplayerBulletController : MonoBehaviourPunCallbacks
{

    Rigidbody rigidBody;

    public AudioClip BulletHitAudio;
    public GameObject bulletImpactEffect;

    [HideInInspector]
    public float bulletSpeed = 15f;
    public int damage = 10;

    [HideInInspector]
    public Photon.Realtime.Player owner;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeBullet(Vector3 originalDirection, Photon.Realtime.Player givenPlayer)
    {
        transform.forward = originalDirection;
        rigidBody.velocity = transform.forward * bulletSpeed;
        owner = givenPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);
        Destroy(gameObject);
    }
}
