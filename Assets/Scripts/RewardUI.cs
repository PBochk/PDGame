using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    private Skill sk;
    private Perk pe;
    public Canvas canvas;
    public Image bg;
    public TextMeshProUGUI description;
    public Button button;
    public Animator animator;

    public void SetRewardDescActive(Skill skill)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isRestrained = true;
        sk = skill;
        bg.gameObject.SetActive(true);
        animator.SetTrigger("RewardStart");
        animator.ResetTrigger("RewardEnd");
        description.text = sk.description;
        button.onClick.AddListener(CloseDesc);
    }

    public void SetRewardDescActive(Perk perk)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isRestrained = true;
        pe = perk;
        bg.gameObject.SetActive(true);
        animator.SetTrigger("RewardStart");
        animator.ResetTrigger("RewardEnd");
        description.text = pe.description;
        button.onClick.AddListener(CloseDesc);
    }

    private void CloseDesc()
    {
        //bg.gameObject.SetActive(false);
        animator.SetTrigger("RewardEnd");
        if (sk != null)
        {
            sk.AddSkill();
            Destroy(sk.gameObject);
        }
        else if (pe != null)
        {
            pe.ActivatePerk();
            Destroy(pe.gameObject);
        }
        button.onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isRestrained = false;
    }

    public void RewardEnd()
    {
        bg.gameObject.SetActive(false);
    }
}
