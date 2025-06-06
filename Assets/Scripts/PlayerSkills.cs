using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public Dictionary<SkillName, Skill> skills;
    public bool isRedirectOn = false;
    public bool isOverloadOn = false;
    private Player player;
    private GameObject temp;
    private void Start()
    {
        skills = new();
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if (skills != null)
        {
            foreach (Skill skill in skills.Values)
            {
                HandleSkill(skill);
            }
        }
    }

    private void HandleSkill(Skill skill)
    {
        if (skill.currentCooldown <= 0)
        {
            if (skill.currentDuration > 0)
            {
                if (!skill.isActive && Input.GetKeyDown(skill.keyCode))
                {
                    SetSkillOn(skill);
                }
                skill.currentDuration -= Time.deltaTime;
            }
            else if (skill.isActive)
            {
                SetSkillOff(skill);
            }
        }
        else if (skill.currentCooldown > 0) 
        {
            skill.currentCooldown -= Time.deltaTime;
        }
    }

    public void SetSkillOn(Skill skill)
    {
        skill.isActive = true;
        switch (skill.skillName)
        {
            case SkillName.Redirect:
                {
                    isRedirectOn = true;
                    break;
                }
            case SkillName.Overload:
                {
                    isOverloadOn = true;
                    break;
                }
            case SkillName.Bomb:
                {
                    Debug.Log("bomb");
                    skill.spawnPosition = player.transform.position;
                    temp = Instantiate(skill.skillObject.gameObject, player.transform.position, Quaternion.identity);
                    break;
                }
        }
    }
        
    public void SetSkillOff(Skill skill)
    {
        skill.isActive = false;
        skill.currentDuration = skill.duration;
        skill.currentCooldown = skill.cooldown;
        switch (skill.skillName)
        {
            case SkillName.Redirect:
                {
                    isRedirectOn = false;
                    break;
                }
            case SkillName.Overload:
                {
                    isOverloadOn = false;
                    break;
                }
            case SkillName.Bomb:
                {
                    Instantiate(skill.effect, skill.spawnPosition, Quaternion.identity);
                    StartCoroutine(WaitForExplosion(skill));
                    break;
                }
        }
    }

    IEnumerator WaitForExplosion(Skill skill)
    {
        yield return new WaitForSeconds(2f);
        Bomb(skill);

    }

    private void Bomb(Skill skill)
    {
        Destroy(temp);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(skill.spawnPosition, skill.range, skill.enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(skill.damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (skills[SkillName.Bomb] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(skills[SkillName.Bomb].spawnPosition, skills[SkillName.Bomb].range);
        }
    }
}
