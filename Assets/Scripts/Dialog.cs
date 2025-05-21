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


    void Start()
    {
        speaker = gameObject;
        var diUI = GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<DialogueUI>();
        windowDialog = diUI.windowDialog;
        textDialog = diUI.textDialog;
        speakerName = diUI.speakerName;
        portrait = diUI.portrait;
        button = diUI.button;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            button.gameObject.SetActive(true);
            button.onClick.AddListener(NextDialog);

            if (numberDialog == message.Length - 1 && !speaker.CompareTag("Character"))
            {
                button.gameObject.SetActive(false);
            }

            windowDialog.SetActive(true);
            textDialog.text = message[numberDialog];
            speakerName.text = names[numberDialog];
            portrait.sprite = portraits[numberDialog];
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            windowDialog.SetActive(false);
            numberDialog = 0;
            button.onClick.RemoveAllListeners();
        }
    }

    public void NextDialog()
    {
        if (numberDialog == message.Length - 1 && speaker.CompareTag("Character"))
        {
            Destroy(speaker);
        }
        numberDialog++;
        textDialog.text = message[numberDialog];
        speakerName.text = names[numberDialog];
        portrait.sprite = portraits[numberDialog];
    }
}