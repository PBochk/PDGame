using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    private Player player;
    private PlayerUI hd;
    public List<Perk> perks;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hd = FindFirstObjectByType<PlayerUI>();
        perks = new();

    }

    public void ActivatePerk(Perk perk)
    {
        HandlePerk(perk);
        perks.Add(perk);
    }

    private void HandlePerk(Perk perk)
    {
        switch (perk.perkName)
        {
            case PerkName.MaxHP:
                {
                    player.maxHealth += perk.perkValue;
                    hd.HealthChanged();
                    break;
                }            
            case PerkName.RegenHP:
                {
                    player.regen += perk.perkValue;
                    break;
                }            
            case PerkName.MovingSpeed:
                {
                    player.movingSpeed += perk.perkValue;
                    break;
                }
        } 
    }
}
