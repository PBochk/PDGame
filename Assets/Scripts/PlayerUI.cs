using System.Xml.XPath;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthDisplay;
    public Text xpDisplay;

    public Image healthBar;
    public Sprite[] healthBarSprites;
    private Player player;
    private PlayerSkills playerSkills;
    private PlayerMeleeAttack attack;
    
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerSkills = player.GetComponent<PlayerSkills>();
        attack = player.GetComponent<PlayerMeleeAttack>();
    }

    void Update()
    {
        //if (playerSkills.skills.Count > 0)
        //{
        //    var bomb = playerSkills.skills[SkillName.Bomb];
        //    healthDisplay.text = bomb.currentCooldown.ToString() + "    " + bomb.currentDuration.ToString();
        //}
        //healthDisplay.text = "Атака: " + attack.timeBtwAttack;
        //healthDisplay.text = player.currentXP.ToString();
        healthDisplay.text = "Здоровье: " + player.health;
        HealthChanged();
        xpDisplay.text = "Очки информации: " + player.currentXP;
    }

    public void HealthChanged()
    {
        var segmentCount = (int)(player.health * 10 / player.maxHealth);
        segmentCount = segmentCount < 0 ? 0 : segmentCount;
        healthBar.sprite = healthBarSprites[segmentCount];
    }
}
