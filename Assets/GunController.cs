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
        GameObject projectile = Instantiate(projectilePrefab, player.position, gun.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Use the gun's forward direction as the direction to apply force
            Vector3 direction = gun.right; // This is the direction the gun is pointing

            // Apply the force in the direction the gun is pointing
            rb.AddForce(direction * speed, ForceMode.VelocityChange);
        }

        Destroy(projectile, 5f);
    }

}
