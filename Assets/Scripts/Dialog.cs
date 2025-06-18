using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dialog : MonoBehaviour
{
    private GameObject speaker;
    private GameObject windowDialog;
    private TextMeshProUGUI textDialog;
    private TextMeshProUGUI speakerName;
    private Image portrait;
    private Button button;
    private Animator anim;

    public string[] message;
    public string[] names;
    public Sprite[] portraits;
    private int numberDialog = 0;

    private bool dialogueEnded;
    private Player player;
    private void Initialize()
    {
        speaker = gameObject;
        var diUI = GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<DialogueUI>();
        windowDialog = diUI.windowDialog;
        textDialog = diUI.textDialog;
        speakerName = diUI.speakerName;
        portrait = diUI.portrait;
        button = diUI.button;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Далее";
        anim = diUI.animator;
        anim.ResetTrigger("endDialogue");
        numberDialog = 0;
        dialogueEnded = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.isRestrained = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        Initialize();
        button.gameObject.SetActive(true);
        button.onClick.AddListener(NextDialog);

        windowDialog.SetActive(true);
        textDialog.text = message[numberDialog];
        speakerName.text = names[numberDialog];
        portrait.sprite = portraits[numberDialog];
        anim.SetTrigger("startDialogue");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("endDialogue");
            player.isRestrained = false;
        }
    }

    public void NextDialog()
    {
        if (numberDialog == message.Length - 1)
        {
            anim.SetTrigger("endDialogue");
            if (speaker.CompareTag("Character"))
            {
                speaker.SetActive(false);
            }
            else
            {
                //windowDialog.SetActive(false);
                if (speaker.CompareTag("Fail"))
                {
                    var portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<Portal>();
                    portal.HandleFail();
                }
                else if (speaker.CompareTag("Win"))
                {
                    var portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<Portal>();
                    portal.HandleWin();
                }
                else
                {
                    var trivia = GameObject.FindGameObjectWithTag("Trivia").GetComponent<TriviaDialogue>();
                    if (trivia.isRightAnswer)
                        trivia.SpawnChest();
                    else
                        trivia.StartTrivia();
                }
            }
            dialogueEnded = true;
            player.isRestrained = false;
            button.onClick.RemoveAllListeners();
        }

        if (!dialogueEnded)
        {
            numberDialog++;
            textDialog.text = message[numberDialog];
            speakerName.text = names[numberDialog];
            portrait.sprite = portraits[numberDialog];
        }
    }
}