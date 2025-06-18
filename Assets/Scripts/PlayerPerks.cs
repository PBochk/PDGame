using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    private Player player;
    private PlayerMeleeAttack melee;
    private RangeAttack range;
    private PlayerUI hd;
    public List<Perk> perks;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        melee = player.GetComponent<PlayerMeleeAttack>();
        range = player.GetComponent<RangeAttack>();
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
            case PerkName.AttackSpeed:
                {
                    melee.attackCooldown *= perk.perkValue;
                    range.shotCooldown *= perk.perkValue;
                    break;
                }
            case PerkName.MeleeDamage:
                {
                    melee.damage += (int)perk.perkValue;
                    break;
                }
            case PerkName.RangeDamage:
                {
                    range.damage += (int)perk.perkValue;
                    break;
                }

        } 
    }
}
