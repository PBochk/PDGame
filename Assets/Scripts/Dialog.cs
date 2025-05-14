using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    public GameObject windowDialog;
    public TextMeshProUGUI textDialog;
    public TextMeshProUGUI speakerName;
    public Image portrait;

    public string[] message;
    public string[] names;
    public Sprite[] portraits;
    private int numberDialog = 0;
    public Button button;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (numberDialog == message.Length - 1)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
                button.onClick.AddListener(NextDialog);
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
        numberDialog++;
        textDialog.text = message[numberDialog];
        speakerName.text = names[numberDialog];
        portrait.sprite = portraits[numberDialog];
        if (numberDialog == message.Length - 1)
        {
            button.gameObject.SetActive(false);
        }
    }
}