using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private GameObject player;
    private bool isFinalCompleted;
    private bool isFinalActive;
    private List<TriviaDialogue> finalTrivias;
    public int triviaNumber = 0;
    private int totalTrivias;
    public GameObject mentorFail;
    public GameObject mentorWin;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isFinalActive)
            {
                isFinalActive = true;
                StartFinalTrivia();
            }
        }
    }



    public void StartFinalTrivia()
    {
        finalTrivias = GameObject.FindGameObjectWithTag("Trivias").GetComponent<TriviaVariants>().spawnedTrivias;
        totalTrivias = finalTrivias.Count;
        HandleFinalTrivia();
    }

    public void HandleFinalTrivia()
    {
        StartCoroutine(Handler());
    }

    IEnumerator Handler()
    {
        yield return new WaitForSeconds(0.5f);
        if (triviaNumber == totalTrivias)
        {
            mentorWin.GetComponent<Dialog>().StartDialogue();
        }
        else
        {
            var current = finalTrivias[triviaNumber];
            triviaNumber++;
            current.isFinal = true;
            current.StartTrivia();
        }
    }

    public void HandleWin()
    {
        StartCoroutine("LevelWon");
    }

    IEnumerator LevelWon()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level2");
    }

    public void HandleFail()
    {
        StartCoroutine("LevelFailed");
    }

    IEnumerator LevelFailed()
    {
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerSkills>().hasBackup = false;
        player.GetComponent<Player>().health = 0;
    }
}
