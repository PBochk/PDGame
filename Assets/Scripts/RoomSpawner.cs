using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class RoomSpawner : MonoBehaviour
{
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

    [SerializeField]
    private bool spawned = false;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Invoke(nameof(Spawn), 0.1f);
    }

    public void Spawn()
    {

        if (direction == Direction.Top)
        {
            rand = Random.Range(0, variants.topRooms.Length);
            Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Bottom)
        {
            rand = Random.Range(0, variants.bottomRooms.Length);
            Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Left)
        {
            rand = Random.Range(0, variants.leftRooms.Length);
            Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
        }
        else if (direction == Direction.Right)
        {
            rand = Random.Range(0, variants.rightRooms.Length);
            Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RoomPoint") && other.GetComponent<RoomSpawner>().direction == Direction.None)
        {
            if (gameObject.GetComponent<RoomSpawner>().direction == Direction.None && !other.CompareTag("BlankRoomPoint"))
            {
                //≈сли спавнитс€ комната поверх комнаты, удал€ет обе и оставл€ет там пустышку. ѕотенциально отрезает кусок уровн€, но лучшего выхода не придумал.
                //ѕри попытке поставить пустышкой комнату с четырьми двер€ми крашитс€
                Debug.Log("Room upon room");

                Destroy(other.gameObject);
                //Instantiate(blankRoom, transform.position, Quaternion.identity);

                //ѕопытка перезагрузить сцену крашит игру, возможно из-за новой схемы управлени€
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); <-- крашит!
                //UPD: игрока было два, возможно ошибка была в этом
            }
            Destroy(gameObject);
        }
    }
}
