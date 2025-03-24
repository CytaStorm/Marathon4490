using System.Collections;
using System.Collections.Generic;
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


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
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

}
