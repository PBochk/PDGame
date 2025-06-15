using UnityEngine;
using UnityEngine.UI;

public class EndHandler : MonoBehaviour
{
    public Button b;
    private void Start()
    {
        b.onClick.AddListener(Application.Quit);
    }
}
