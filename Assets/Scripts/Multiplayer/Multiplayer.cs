using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour, IPunObservable
{
    [HideInInspector]
    public float movementSpeed = 10f;
    public float fireRate = 0.75f;
    public int health = 100;
    public Slider healthBar;

    Rigidbody rigidBody;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    float nextFire;

    public AudioClip playerShootingAudio;
    public GameObject bulletFiringEffect;

    PhotonView photonView;


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        PhotonTransformView transformView = GetComponent<PhotonTransformView>();
        photonView.ObservedComponents = new List<Component> { transformView };
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraTracking>().enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        Move();
        if (Input.GetKey(KeyCode.Space))
            photonView.RPC("Fire", RpcTarget.AllViaServer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            MultiplayerBulletController bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();
            TakeDamage(bullet);
        }
    }

    void TakeDamage(MultiplayerBulletController bullet)
    {
        health -= bullet.damage;
        healthBar.value = health;
        if (health <= 0)
        {
            bullet.owner.AddScore(1);
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        health = 100;
        healthBar.value = health;
    }

    void Move()
    { 
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rigidBody.MovePosition(rigidBody.position + movementDir);
    }
    [PunRPC]
    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<MultiplayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);
            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            healthBar.value = health;
        }
    }
}
