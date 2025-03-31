using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField]
    private Slider cooldownSlider;

    private float cooldownTimer;

    public void Start()
    {
        cooldownWaiting = false;
        cooldownTimer = 0f;

        if (cooldownSlider != null)
        {
            cooldownSlider.value = 0f;
        }
    }


    private void Update()
    {
        if (cooldownWaiting)
        {
            // Updates the cooldown timer
            cooldownTimer -= Time.deltaTime;
            cooldownSlider.value = Mathf.Clamp01(cooldownTimer / cooldownTimeMilliseconds * 1000);

            // Optional: You can add a visual effect like changing color based on progress
            if (cooldownSlider.value == 1f)
            {
                cooldownSlider.GetComponentInChildren<Image>().color = Color.green; // Fully charged color
            }
            else
            {
                cooldownSlider.GetComponentInChildren<Image>().color = Color.red; // Cooldown in progress color
            }
        }
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
        cooldownTimer = cooldownTime / 1000f;
        await Task.Delay(cooldownTime);
        // Debug.Log("Cooldown Ended");
        cooldownWaiting = false;
        cooldownTimer = 0f;
        cooldownSlider.value = 0f;
    }

    /*
    Code to be put in playercontroller
      Fields:

    // GunController gunController;


    START:

	gunController = GetComponent<GunController>();
    if (gunController == null)
    {
        Debug.LogError("GunController not found on the same GameObject!");
    }
	else
	{
        Debug.Log("GunController found");
    }
	

    UPDATE:

    if (Input.GetMouseButtonDown(0))    // To be changed to new input system later
    {
        if (gunController.cooldownWaiting)
            {
                Debug.Log("Cooldown is active!");
            }
            else
            {
                _rigidBody.AddForce(forceDirection);
            } 
    }

    */


}
