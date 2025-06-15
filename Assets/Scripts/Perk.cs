using UnityEngine;

public class Perk : MonoBehaviour
{
    private Player player;
    private RewardUI perkUI;
    private PlayerPerks playerPerks; 

    public PerkName perkName;
    public string description;
    public float perkValue;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        perkUI = GameObject.FindGameObjectWithTag("RewardUI").GetComponent<RewardUI>();
        playerPerks = FindFirstObjectByType<Player>().GetComponent<PlayerPerks>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            perkUI.SetRewardDescActive(this);
        }
    }

    public void ActivatePerk() 
    { 
        playerPerks.ActivatePerk(this);
    }
}
