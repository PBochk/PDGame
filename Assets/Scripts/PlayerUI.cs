using System.Xml.XPath;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthBar;
    public Sprite[] healthBarSprites;
    public Image xpBar;
    public Sprite[] xpBarSprites;
    private Player player;
    private RoomVariants rv;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        rv = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    void Update()
    {
        HealthChanged();
    }

    public void HealthChanged()
    {
        var segmentCount = (int)(player.health * 10 / player.maxHealth);
        segmentCount = segmentCount < 0 ? 0 : segmentCount;
        healthBar.sprite = healthBarSprites[segmentCount];
    }

    public void XPChanged()
    {
        var XPToEnd = rv.XPToEnd;
        Debug.Log(player.currentXP + "   " + XPToEnd);
        var segmentCount = (int)(player.currentXP * 20 / XPToEnd);
        segmentCount = segmentCount < 0 ? 0 : segmentCount;
        if (segmentCount < 0)
            segmentCount = 0;
        else if (segmentCount > 20)
            segmentCount = 20;
        xpBar.sprite = xpBarSprites[segmentCount];
    }
}
