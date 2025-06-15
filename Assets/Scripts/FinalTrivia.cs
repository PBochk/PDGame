//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class FinalTrivia : MonoBehaviour
//{
//    private List<GameObject> finalTrivias;

//    private GameObject windowDialog;
//    private TextMeshProUGUI question;
//    private TextMeshProUGUI rightAnswer;
//    private TextMeshProUGUI wrongAnswer;
//    private TextMeshProUGUI reset;
//    private Button rightButton;
//    private Button wrongButton;
//    private Button resetButton;

//    public string qstText;
//    public string righAnsText;
//    public string wrongAnsText;
//    private int rightNumber;

//    public string rightText;
//    public string wrongText;

//    [HideInInspector] public bool isRightAnswer;

//    private void Start()
//    {
//        finalTrivias = GameObject.FindGameObjectWithTag("Trivias").GetComponent<TriviaVariants>().spawnedTrivias;
//        var tUI = GameObject.FindGameObjectWithTag("TriviaDialogue").GetComponent<TriviaUI>();
//        windowDialog = tUI.windowDialog;
//        question = tUI.question;

//        rightNumber = Random.Range(0, 2);
//        if (rightNumber == 0)
//        {
//            rightAnswer = tUI.answer1;
//            wrongAnswer = tUI.answer2;

//            rightButton = tUI.button1;
//            wrongButton = tUI.button2;
//        }
//        else
//        {
//            rightAnswer = tUI.answer2;
//            wrongAnswer = tUI.answer1;

//            rightButton = tUI.button2;
//            wrongButton = tUI.button1;
//        }
//        resetButton = tUI.resetButton;
//        reset = tUI.reset;
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            StartTrivia();
//        }
//    }

//    public void StartTrivia()
//    {
//        rightButton.gameObject.SetActive(true);
//        wrongButton.gameObject.SetActive(true);

//        rightButton.onClick.AddListener(RightAnswer);
//        wrongButton.onClick.AddListener(WrongAnswer);

//        windowDialog.SetActive(true);
//        question.text = qstText;
//        rightAnswer.text = righAnsText;
//        wrongAnswer.text = wrongAnsText;
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            windowDialog.SetActive(false);
//            rightButton.onClick.RemoveAllListeners();
//            wrongButton.onClick.RemoveAllListeners();
//            resetButton.onClick.RemoveAllListeners();
//            resetButton.gameObject.SetActive(false);
//        }
//    }

//    public void WrongAnswer()
//    {
//        isRightAnswer = false;
//        question.text = wrongText;
//        rightButton.onClick.RemoveAllListeners();
//        wrongButton.onClick.RemoveAllListeners();
//        rightButton.gameObject.SetActive(false);
//        wrongButton.gameObject.SetActive(false);
//        resetButton.gameObject.SetActive(true);
//    }




//    public void RightAnswer()
//    {
//        isRightAnswer = true;
//        question.text = rightText;
//        rightButton.onClick.RemoveAllListeners();
//        wrongButton.onClick.RemoveAllListeners();
//        rightButton.gameObject.SetActive(false);
//        wrongButton.gameObject.SetActive(false);
//        resetButton.gameObject.SetActive(true);
//        reset.text = "Открыть";
//    }

//}
