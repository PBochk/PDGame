using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Skill : MonoBehaviour
{
    private PlayerSkills playerSkills;
    public SkillName skillName;
    public float duration;
    [HideInInspector] public float currentDuration;
    public float cooldown;
    [HideInInspector] public float currentCooldown = 0;
    [HideInInspector] public bool isActive = false;
    public KeyCode keyCode;
    public string description;
    private RewardUI skillUI;
    public GameObject skillObject;
    public GameObject effect;
    public LayerMask enemy;
    public int damage;
    [HideInInspector] public Vector3 spawnPosition;
    public float range;
    private void Start()
    {
        playerSkills = FindFirstObjectByType<Player>().GetComponent<PlayerSkills>();
        skillUI = GameObject.FindGameObjectWithTag("RewardUI").GetComponent<RewardUI>();
        currentDuration = duration;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillUI.SetRewardDescActive(this);
        }
    }

    public void AddSkill()
    {
        playerSkills.skills[skillName] = this;
    }

    public void Explode()
    {
        Destroy(skillObject.gameObject);
    }
}
