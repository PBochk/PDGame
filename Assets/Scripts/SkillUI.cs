using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private Skill sk;
    public Canvas canvas;
    public Image bg;
    public TextMeshProUGUI description;
    public Button button;

    public void SetSkillDescActive(Skill skill)
    {
        sk = skill;
        bg.gameObject.SetActive(true);
        description.text = sk.description;
        button.onClick.AddListener(CloseDesc);
    }

    private void CloseDesc()
    {
        bg.gameObject.SetActive(false);
        sk.AddSkill();
    }
}
