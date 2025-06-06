using System.Linq;
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
    [HideInInspector] public bool isActive;
    public KeyCode keyCode;
    public string description;
    private SkillUI skillUI;
    public GameObject skillObject;
    public GameObject effect;
    public LayerMask enemy;
    public int damage;
    [HideInInspector] public Vector3 spawnPosition;
    public float range;
    private void Start()
    {
        playerSkills = FindFirstObjectByType<Player>().GetComponent<PlayerSkills>();
        skillUI = GameObject.FindGameObjectWithTag("SkillUI").GetComponent<SkillUI>();
        currentDuration = duration;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillUI.SetSkillDescActive(this);
        }
    }

    public void AddSkill()
    {
        playerSkills.skills[skillName] = this;
        Destroy(gameObject);
    }

    public void Explode()
    {
        Destroy(skillObject.gameObject);
    }
}
