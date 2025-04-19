using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField, Min(1), Tooltip("Number of Rooms & Connectors to be Generator. *Not including Spawn & Objective")] 
    int _levelSize = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
