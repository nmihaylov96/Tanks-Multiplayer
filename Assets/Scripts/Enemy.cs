using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
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

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            transform.LookAt(other.transform);
            Fire();
        }
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
            EnemyDied();
    }

    void EnemyDied()
    {
        gameObject.SetActive(false);
        if (OnEnemyKilled != null)
            OnEnemyKilled.Invoke();
    }
}
