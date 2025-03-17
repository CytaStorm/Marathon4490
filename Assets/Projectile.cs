using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectiveManager oManager;

    void Start()
    {
        // Finds the ObjectiveManager in the scene
        oManager = FindObjectOfType<ObjectiveManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Objective"))
        {
            oManager.CompletedObjectives++;
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
