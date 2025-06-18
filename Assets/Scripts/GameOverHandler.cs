using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Button b;
    private void Start()
    {
        b.onClick.AddListener(Reload);
    }
    private void Reload()
    {
        SceneManager.LoadScene("Level");
    }
}
