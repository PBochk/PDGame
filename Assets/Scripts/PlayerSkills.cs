using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public Dictionary<SkillName, Skill> skills;
    public bool isRedirectOn = false;
    public bool isOverloadOn = false;
    public bool hasBackup = false;
    private Player player;
    private PlayerUI healthDisplay;

    private GameObject temp;
    private GameObject backup;
    private GameObject cam;
    private Vector3 camPos;
    private void Start()
    {
        skills = new();
        player = FindFirstObjectByType<Player>();
        healthDisplay = FindFirstObjectByType<PlayerUI>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
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
            if (!skill.isActive && Input.GetKeyDown(skill.keyCode))
            {
                SetSkillOn(skill);
            }
            else if (skill.isActive)
            {
                if (skill.currentDuration <= 0)
                {
                    SetSkillOff(skill);
                }
                else
                {
                    skill.currentDuration -= Time.deltaTime;
                }
            }
        }
        else
        {
            skill.currentCooldown -= Time.deltaTime;
        }
    }

    public void SetSkillOn(Skill skill)
    {
        skill.isActive = true;
        Debug.Log(skill.skillName);
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
                    skill.spawnPosition = player.transform.position;
                    temp = Instantiate(skill.skillObject.gameObject, player.transform.position, Quaternion.identity);
                    break;
                }
            case SkillName.Backup:
                {
                    if(!hasBackup)
                    {
                        backup = Instantiate(skill.skillObject.gameObject, player.transform.position, Quaternion.identity);
                    }
                    hasBackup = true;
                    camPos = cam.transform.position;
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
            case SkillName.Backup:
                {
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

    public void Revive()
    {
        Debug.Log("этоябэкап");
        healthDisplay.HealthChanged();
        hasBackup = false;
        player.transform.position = backup.transform.position;
        Destroy(backup);
        cam.transform.position = camPos;
    }
}
