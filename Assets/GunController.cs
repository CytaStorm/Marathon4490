using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool cooldownWaiting;

    public void Start()
    {
        cooldownWaiting = false;
    }


    private void Update()
    {
        
    }

    public void ShootProjectile(InputAction.CallbackContext ShootProjectile) // InputAction.CallbackContext Shoot
    {
        if (ShootProjectile.performed && !cooldownWaiting)
        {
            // Instantiates the projectile at the players position and rotation
            GameObject projectile = Instantiate(projectilePrefab, player.position, gun.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            Debug.Log("It shot");

            if (rb != null)
            {
                Vector3 direction = gun.right;

                // Applies the force in the direction the gun is pointing
                rb.AddForce(direction * speed, ForceMode.VelocityChange);
            }

            // Destroys the projectile after 5 seconds
            Destroy(projectile, 5f);

            StartCooldown(cooldownTimeMilliseconds);
        }
    }


    async void StartCooldown(int cooldownTime)
    {
        cooldownWaiting = true;
        await Task.Delay(cooldownTime);
        // Debug.Log("Cooldown Ended");
        cooldownWaiting = false;
    }

}
