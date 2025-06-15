using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class RoomSpawner : MonoBehaviour
{
    public GameObject parentRoom; 
    public GameObject blankRoom;
    public Direction direction;

    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }

    private RoomVariants variants;
    private int rand;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Invoke(nameof(Spawn), 0.1f);
    }

    public void Spawn()
    {
        GameObject room = null;
        if (direction == Direction.Top)
        {
            rand = Random.Range(0, variants.topRooms.Length);
            room = Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Bottom)
        {
            rand = Random.Range(0, variants.bottomRooms.Length);
            room = Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Left)
        {
            rand = Random.Range(0, variants.leftRooms.Length);
            room = Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Right)
        {
            rand = Random.Range(0, variants.rightRooms.Length);
            room = Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
        }
        if (direction != Direction.None)
        {
            variants.CheckGeneration(room);
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RoomPoint") && other.GetComponent<RoomSpawner>().direction == Direction.None)
        {
            if (gameObject.GetComponent<RoomSpawner>().direction == Direction.None && !other.CompareTag("BlankRoomPoint"))
            {
                Debug.Log("Room upon room");
                variants.rooms.Remove(parentRoom);
                Destroy(parentRoom);
                Instantiate(blankRoom, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
