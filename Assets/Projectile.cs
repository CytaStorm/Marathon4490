using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        // Destroy(gameObject);
    }
}
