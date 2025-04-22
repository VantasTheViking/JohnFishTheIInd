using UnityEngine;

public class Room : MonoBehaviour
{
    private Bounds combinedBounds;

    void Awake()
    {


        CalculateCombinedBounds();
    }

    void CalculateCombinedBounds()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            combinedBounds = new Bounds(transform.position, Vector3.zero);
            return;
        }

        combinedBounds = colliders[0].bounds;
        for (int i = 1; i < colliders.Length; i++)
        {
            combinedBounds.Encapsulate(colliders[i].bounds);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(combinedBounds.center, combinedBounds.size);
    }
}
