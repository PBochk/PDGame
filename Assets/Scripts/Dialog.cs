using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    private GameObject speaker;
    private GameObject windowDialog;
    private TextMeshProUGUI textDialog;
    private TextMeshProUGUI speakerName;
    private Image portrait;
    private Button button;

    public string[] message;
    public string[] names;
    public Sprite[] portraits;
    private int numberDialog = 0;

    public bool dialogueEnded;

    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        speaker = gameObject;
        var diUI = GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<DialogueUI>();
        windowDialog = diUI.windowDialog;
        textDialog = diUI.textDialog;
        speakerName = diUI.speakerName;
        portrait = diUI.portrait;
        button = diUI.button;

        numberDialog = 0;
        dialogueEnded = false;
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            windowDialog.SetActive(false);
        }
    }

    public void NextDialog()
    {
        
        if (numberDialog == message.Length - 1)
        {
            if (speaker.CompareTag("Character"))
            {
                speaker.SetActive(false);
            }
            else
            {
                windowDialog.SetActive(false);
                var trivia = GameObject.FindGameObjectWithTag("Trivia").GetComponent<TriviaDialogue>();
                if (trivia.isRightAnswer)
                    trivia.SpawnChest();
                else
                    trivia.StartTrivia();
            }
            dialogueEnded = true;
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