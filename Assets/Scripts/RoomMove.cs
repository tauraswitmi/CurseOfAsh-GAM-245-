using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMove : MonoBehaviour
{
    public int roomFrom;
    public int roomTo;
    
    public Vector2 cameraChangeMax;
    public Vector2 cameraChangeMin;

    public Vector3 playerChange;

    private CameraMovement cam;

    public int spawn;

    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.minPosition += cameraChangeMin;
            cam.maxPosition += cameraChangeMax;
            other.transform.position += playerChange;

            other.GetComponent<PlayerMovement>().enemyRoom.DeactivateEnemies(roomFrom);
            other.GetComponent<PlayerMovement>().currentRoom = roomTo;
            other.GetComponent<PlayerMovement>().enemyRoom.ActivateEnemies(roomTo);
            other.GetComponent<PlayerMovement>().spawnId = spawn;

        }
    }

    public void RoomCameraChange(float maxX,  float maxY, float minX, float minY)
    {
        Vector2 maximum = new Vector2 (maxX, maxY);
        Vector2 minimum = new Vector2 (minX, minY);
        cam.minPosition += minimum;
        cam.maxPosition += maximum;
    }
}
