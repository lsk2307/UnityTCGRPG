using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleParty : MonoBehaviour
{
    public Slider skillCool;
    public Skill skill;
    public GameObject sturnIcon;
    public GameObject buffIcon;
    public GameObject button;

    public bool sturn;
    public bool endding;
    public int partyNum;
    
    bool targetting;
    float targettingTime;
    float coolTime = 0.1f;
    float sturnTime;

    private void Awake()
    {
        button.SetActive(true);
    }

    private void Update()
    {
        if (endding) return;

        if(skillCool.value != 0 && !sturn)
        {
            skillCool.value -= coolTime * Time.deltaTime;
        }

        if(sturn)
        {
            sturnTime -= Time.deltaTime;

            if(sturnTime <= 0)
            {
                sturn = false;
                sturnIcon.SetActive(false);
            }
        }

        if(targetting)
        {
            targettingTime -= Time.deltaTime;

            if(targettingTime <= 0)
            {
                targetting = false;
                buffIcon.SetActive(false);
            }
        }
    }

    public void UseSkill()
    {
        if (endding) return;
        if (skillCool.value != 0) return;
        skillCool.value = 1;
        skill.SkillUse(partyNum);
    }


    public void Sturn(float time)
    {
        sturn = true;
        sturnIcon.SetActive(true);
        sturnTime = time;
    }

    public void TargetOn(float time)
    {
        targetting = true;
        targettingTime = time;
        buffIcon.SetActive(true);
    }
}
