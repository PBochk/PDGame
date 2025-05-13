using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    public GameObject windowDialog;
    public TextMeshProUGUI textDialog;
    public TextMeshProUGUI speakerName;
    public string[] message;
    public string[] names;
    public int numberDialog = 0;
    public Button button;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
        if (numberDialog == message.Length - 1)
        {
            button.gameObject.SetActive(false);
        }
    }
}