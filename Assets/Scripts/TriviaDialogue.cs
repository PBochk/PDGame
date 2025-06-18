using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriviaDialogue : MonoBehaviour
{
    [SerializeField] private AudioClip rightAnswerSound;
    [SerializeField] private AudioClip wrongAnswerSound;
    private AudioSource audioSource;
    private Animator animator;
    private GameObject windowDialog;
    private TextMeshProUGUI question;
    private TextMeshProUGUI rightAnswer;
    private TextMeshProUGUI wrongAnswer;
    private TextMeshProUGUI reset;
    private Button rightButton;
    private Button wrongButton;
    private Button resetButton;

    public string qstText;
    public string righAnsText;
    public string wrongAnsText;
    private int rightNumber;

    public string rightText;
    public string wrongText;

    public GameObject mentorRight;
    private Dialog mentorRightDialogue;    
    public GameObject mentorWrong;
    private Dialog mentorWrongDialogue;
    private Dialog mentorFailDialogue;    

    [HideInInspector] public bool isRightAnswer;
    public bool isFinal;
    private Portal portal;
    private Player player;

    private void Start()
    {
        isFinal = false;
    }

    private void Initialize()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        if (!isFinal)
        {
            mentorRightDialogue = mentorRight.GetComponent<Dialog>();
            mentorWrongDialogue = mentorWrong.GetComponent<Dialog>();
        }
        else
        {
            portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<Portal>();
            mentorFailDialogue = portal.mentorFail.GetComponent<Dialog>();
        }

        var tUI = GameObject.FindGameObjectWithTag("TriviaDialogue").GetComponent<TriviaUI>();
        windowDialog = tUI.windowDialog;
        question = tUI.question;

        rightNumber = Random.Range(0, 2);
        if (rightNumber == 0)
        {
            rightAnswer = tUI.answer1;
            wrongAnswer = tUI.answer2;

            rightButton = tUI.button1;
            wrongButton = tUI.button2;
        }
        else
        {
            rightAnswer = tUI.answer2;
            wrongAnswer = tUI.answer1;

            rightButton = tUI.button2;
            wrongButton = tUI.button1;
        }
        resetButton = tUI.resetButton;
        reset = tUI.reset;
        animator = tUI.animator;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.isRestrained = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartTrivia();            
        }
    }

    public void StartTrivia()
    {
        Initialize();
        rightButton.gameObject.SetActive(true);
        wrongButton.gameObject.SetActive(true);

        rightButton.onClick.AddListener(RightAnswer);
        wrongButton.onClick.AddListener(WrongAnswer);

        windowDialog.SetActive(true);
        animator.ResetTrigger("EndTrivDial");

        animator.SetTrigger("StartTrivDial");

        question.text = qstText;
        rightAnswer.text = righAnsText;
        wrongAnswer.text = wrongAnsText;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.isRestrained = false;
            animator.SetTrigger("EndTrivDial");
            //windowDialog.SetActive(false);
            rightButton.onClick.RemoveAllListeners();
            wrongButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            resetButton.gameObject.SetActive(false);
        }
    }


    public void RightAnswer()
    {
        audioSource.PlayOneShot(rightAnswerSound);
        isRightAnswer = true;
        question.text = rightText;
        rightButton.onClick.RemoveAllListeners();
        wrongButton.onClick.RemoveAllListeners();
        rightButton.gameObject.SetActive(false);
        wrongButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);
        if (!isFinal)
        {
            reset.text = "Открыть";
        }
        else
        {
            reset.text = "Продолжить";
        }
        resetButton.onClick.AddListener(ResetQuestion);
    }

    public void WrongAnswer()
    {
        audioSource.PlayOneShot(wrongAnswerSound);
        isRightAnswer = false;
        question.text = wrongText;
        rightButton.onClick.RemoveAllListeners();
        wrongButton.onClick.RemoveAllListeners();
        rightButton.gameObject.SetActive(false);
        wrongButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);
        if (!isFinal)
        {
            reset.text = "Помощь!";
        }
        else
        {
            reset.text = "Продолжить";
        }
        resetButton.onClick.AddListener(ResetQuestion);
    }

    public void ResetQuestion()
    {
        resetButton.onClick.RemoveAllListeners();
        resetButton.gameObject.SetActive(false);
        animator.SetTrigger("EndTrivDial");
        //windowDialog.SetActive(false);
        if (!isFinal)
        {
            if (isRightAnswer)
            {
                mentorRightDialogue.StartDialogue();
            }
            else
            {
                mentorWrongDialogue.StartDialogue();
            }
        }
        else
        {
            if (isRightAnswer)
            {
                Debug.Log("Right answer, next question");
                portal.HandleFinalTrivia();
            }
            else
            {
                mentorFailDialogue.StartDialogue();
            }
        }
    }

    public void SpawnChest()
    {
        var rewardPool = GameObject.FindGameObjectWithTag("Rewards").GetComponent<RewardPool>();
        var rewards = rewardPool.rewardPool;
        if (rewards.Count > 0)
        {
            var received = rewardPool.receivedRewards;
            var reward = rewards[Random.Range(0, rewards.Count)];
            received.Add(reward);
            if (!reward.CompareTag("Perk"))
            {
                rewards.Remove(reward);
            }
            Instantiate(reward, transform.position, Quaternion.identity);
        }
        resetButton.onClick.RemoveAllListeners();
        resetButton.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
