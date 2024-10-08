using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            TakeDamage(bullet.damage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
            PlayerDied();
    }

    void PlayerDied()
    {
        gameObject.SetActive(false);
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

    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<BulletController>()?.InitializeBullet(transform.rotation * Vector3.forward);
            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
        }
    }
}
