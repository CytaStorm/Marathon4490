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
            cooldownSlider.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (cooldownWaiting)
        {
            // Updates the cooldown timer
            cooldownTimer -= Time.deltaTime;
            cooldownSlider.value = Mathf.Clamp01(cooldownTimer / cooldownTimeMilliseconds * 1000);

            // Changes color/active based on how filled it is
            if (cooldownSlider.value == 0f)
            {
                // cooldownSlider.GetComponentInChildren<Image>().color = Color.green;
                cooldownSlider.gameObject.SetActive(false);
            }
            else
            {
                cooldownSlider.gameObject.SetActive(true);
                cooldownSlider.GetComponentInChildren<Image>().color = Color.black; // Cooldown in progress color
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
    public void ShootProjectile(InputAction.CallbackContext ShootProjectile)
	{
		if (gunController.cooldownWaiting)
        {
            Debug.Log("Cooldown is active!");
        }
        else
        {
			Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 forceDirection = transform.position - mouseLocation;
			forceDirection.z = 0;
			forceDirection.Normalize();

            // Multiply direction vector by force scalar
            forceDirection *= shotStrength;
            print(forceDirection);

            _rigidBody.AddForce(forceDirection);

            Debug.Log("Thing shot hopefully");
        }
	}
    */


}
