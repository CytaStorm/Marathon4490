using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    public GameObject projectilePrefab;

    [SerializeField]
    public Transform player;

    [SerializeField]
    public Transform gun;

    [SerializeField]
    public float speed;

    [SerializeField]
    public int cooldownTimeMilliseconds;

    public bool cooldownWaiting = false;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))// && !cooldownWaiting
        {
            // Debug.Log("Cooldown Started");
            ShootProjectile();
            // StartCooldown(cooldownTimeMilliseconds);
        }
    }

    void ShootProjectile()
    {
        // Instantiates the projectile at the players position and rotation
        GameObject projectile = Instantiate(projectilePrefab, player.position, gun.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = gun.right;

            // Applies the force in the direction the gun is pointing
            rb.AddForce(direction * speed, ForceMode.VelocityChange);
        }

        // Destroys the projectile after 5 seconds
        Destroy(projectile, 5f);
    }

    async void StartCooldown(int cooldownTime)
    {
        cooldownWaiting = true;
        await Task.Delay(cooldownTime);
        // Debug.Log("Cooldown Ended");
        cooldownWaiting = false;
    }

}
