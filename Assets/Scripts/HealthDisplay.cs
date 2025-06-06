using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public Text healthDisplay;

    public Image healthBar;
    public Sprite[] healthBarSprites;
    private Player player;
    private PlayerMeleeAttack attack;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        attack = player.GetComponent<PlayerMeleeAttack>();
    }
    void Update()
    {
        healthDisplay.text = "Здоровье: " + player.health;
        //healthDisplay.text = "Атака: " + attack.timeBtwAttack;
    }

    public void HealthChanged()
    {
        var segmentCount = player.health * 10 / player.maxHealth;
        segmentCount = segmentCount < 0 ? 0 : segmentCount;
        healthBar.sprite = healthBarSprites[segmentCount];
    }
}
