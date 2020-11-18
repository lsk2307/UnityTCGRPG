using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Battle battle;
    public BattleParty[] bp;
    public GameObject damageFont;
    public GameObject skillBarObj;
    public GameObject sturnIcon;
    public Transform playerHPBar;
    Slider skillBar;
    public Text skillName;
    public Text damageText;
    public int bossHP;
    public bool endding;

    int stageNum;
    int damage;
    int pattern;
    int targettingNum;
    float targettingTime;
    float attackTimer;
    float skillCasting;
    float skillUsing;

    bool attacking;
    bool canclling;
    bool targetting;
    bool sturnning;

    private void Awake()
    {
        skillBar = skillBarObj.GetComponent<Slider>();
        attacking = false;
        pattern = 0;
        attackTimer = 0;
        skillCasting = 0;
        skillUsing = 0;
        canclling = false;
        stageNum = PlayerDataBase.instance.stageNum;

    }

    private void Update()
    {
        if (endding) return;

        switch (stageNum)
        {
            case 1:
                SharkEnem();
                break;
        }

        if(targetting)
        {
            targettingTime -= Time.deltaTime;
            if (targettingTime <= 0)
                targetting = false;
        }

        if(sturnning && attackTimer >= 0)
        {
            sturnIcon.SetActive(false);
            sturnning = false;
        }
    }

    void SharkEnem()
    {
        if(!attacking)
        attackTimer += Time.deltaTime;

        switch(pattern)
        {
            case 0:
                if(attackTimer >= 1f)
                {
                    attacking = true;
                    attackTimer = 0;
                    skillCasting = 0;
                    skillUsing = 5f;
                    skillBarObj.SetActive(true);
                    skillBar.value = 0;
                    skillName.text = "베기";
                }
                if(attacking)
                {
                    skillCasting += Time.deltaTime;
                    skillBar.value = skillCasting / skillUsing;

                    if(skillCasting >= skillUsing)
                    {
                        attacking = false;
                        skillBarObj.SetActive(false);
                        damage = 300 - PlayerDataBase.instance.partyDef;
                        if (damage < 0) damage = 0;
                        battle.playerCurrentHP -= damage;
                        battle.HPBarChange(true);
                        pattern = 1;
                        battle.EffectOn(0, true);
                        damageFont.SetActive(false);
                        damageFont.SetActive(true);
                        damageFont.transform.position = playerHPBar.position;
                        damageText.text = "-" + damage;
                        damageText.color = Color.red;
                    }
                }
                break;
            case 1:
                if (attackTimer >= 1f)
                {
                    attacking = true;
                    attackTimer = 0;
                    skillCasting = 0;
                    skillUsing = 3f;
                    skillBarObj.SetActive(true);
                    skillBar.value = 0;
                    skillName.text = "찌르기";
                }
                if (attacking)
                {
                    skillCasting += Time.deltaTime;
                    skillBar.value = skillCasting / skillUsing;

                    if (skillCasting >= skillUsing)
                    {
                        attacking = false;
                        skillBarObj.SetActive(false);
                        damage = 150 - PlayerDataBase.instance.partyDef;
                        if (damage < 0) damage = 0;
                        battle.playerCurrentHP -= damage;
                        battle.HPBarChange(true);

                        if(targetting)
                        {
                            bp[targettingNum].Sturn(3f);
                        }
                        else
                        {
                            int ran = Random.Range(0, PlayerDataBase.instance.PartyIndex.Count);
                            bp[ran].Sturn(3f);
                        }
                        battle.EffectOn(0, true);
                        damageFont.SetActive(false);
                        damageFont.SetActive(true);
                        damageFont.transform.position = playerHPBar.position;
                        damageText.text = "-" + damage;
                        damageText.color = Color.red;
                        pattern = 2;
                    }
                }
                break;
            case 2:
                if (attackTimer >= 2f)
                {
                    attacking = true;
                    attackTimer = 0;
                    skillCasting = 0;
                    skillUsing = 7f;
                    skillBarObj.SetActive(true);
                    skillBar.value = 0;
                    skillName.text = "!날카로운 이빨!";
                    canclling = true;
                }
                if (attacking)
                {
                    skillCasting += Time.deltaTime;
                    skillBar.value = skillCasting / skillUsing;

                    if (skillCasting >= skillUsing)
                    {
                        attacking = false;
                        skillBarObj.SetActive(false);
                        damage = 600 - PlayerDataBase.instance.partyDef;
                        if (damage < 0) damage = 0;
                        battle.playerCurrentHP -= damage;
                        battle.HPBarChange(true);
                        pattern = 0;
                        canclling = false;
                        battle.EffectOn(0, true);
                        damageFont.SetActive(false);
                        damageFont.SetActive(true);
                        damageFont.transform.position = playerHPBar.position;
                        damageText.text = "-" + damage;
                        damageText.color = Color.red;   
                    }
                }
                break;
        }
    }

    public void Stun(float time)
    {
        if(!canclling && attacking)
        {
            sturnning = true;
            attacking = false;
            attackTimer = 0;
            skillCasting = 0;
            skillBarObj.SetActive(false);
            pattern++;
            attackTimer -= time;
            sturnIcon.SetActive(true);
        }
    }

    public void TarggettingOn(int partyNum, float time)
    {
        targetting = true;
        targettingNum = partyNum;
        targettingTime = time;
    }
}
