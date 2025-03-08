using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    public GameObject projectilePrefab;

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
        GameObject projectile = Instantiate(projectilePrefab, gun.position, gun.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Destroy(projectile, 5f);
    }

}
