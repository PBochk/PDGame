using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    public List<GameObject> rooms;

    private Canvas loadingScreen;

    private bool isChecking = false;
    private Player player;
    public float timeToLose;
    public GameObject mentorLose;
    private Dialog loseDialogue;
    public int XPToEnd = 300;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        loseDialogue = mentorLose.GetComponent<Dialog>();
        var firstRoom = GameObject.FindGameObjectWithTag("FirstRoom");
        rooms.Add(firstRoom);
        loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<Canvas>();
    }

    private void Update()
    {
        timeToLose -= Time.deltaTime;
        if (timeToLose <= 0 )
        {
            loseDialogue.StartDialogue();
            StartCoroutine(TimesOut());
        }
    }

    IEnumerator TimesOut()
    {
        yield return new WaitForSeconds(2f);
        player.GetComponent<PlayerSkills>().hasBackup = false;
        player.health = 0;
    }

    public void CheckGeneration(GameObject newRoom)
    {
        rooms.Add(newRoom);
        if (rooms.Count > 13)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (!isChecking)
        {
            isChecking = true;
            StartCoroutine(WaitForGeneration());
        }

    }

    IEnumerator WaitForGeneration()
    {
        yield return new WaitForSeconds(1f);
        CorrectGeneration();
    }

    public void CorrectGeneration()
    {
        var roomSpawners = GameObject.FindGameObjectsWithTag("RoomPoint");
        bool isGenerated = true;
        foreach (GameObject rs in roomSpawners)
        {
            if (rs.GetComponent<RoomSpawner>().direction != RoomSpawner.Direction.None)
            {
                isGenerated = false;
                break;
            }
        }
        if (isGenerated && rooms.Count < 9)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (isGenerated)
        {
            loadingScreen.gameObject.SetActive(false);
            var m = FindFirstObjectByType<AudioSource>();
            m.Play();
            player.isRestrained = false;
        }
        //Debug.Log(rooms.Count + " " + isGenerated);
        isChecking = false;
        XPToEnd = 2 * 4 * (rooms.Count - 1);
    }
}
