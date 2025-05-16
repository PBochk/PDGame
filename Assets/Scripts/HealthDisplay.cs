using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int health;
    public Text healthDisplay;

    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        health = player.health;
    }
    void Update()
    {
        healthDisplay.text = "Çהמנמגüו: " + health;
    }

    public void DamageTaken(int damage)
    {
        health -= damage;
    }
}
