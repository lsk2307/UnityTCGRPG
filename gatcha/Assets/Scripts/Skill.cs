using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Battle battle;
    public Enemy enemy;
    public BattleParty[] bp;
    public Transform partyBar;
    public GameObject damage;
    public Text damageFont;

    int partyNumber;

    public void SkillUse(int partyNum)
    {
        partyNumber = partyNum;
        SkillSet(PlayerDataBase.instance.PartyIndex[partyNum].playerNum, PlayerDataBase.instance.PartyIndex[partyNum].playerRare);
    }

    void SkillSet(int num, bool rare)
    {
        int atk = PlayerDataBase.instance.GetPartyAtk(partyNumber, rare);
        float cri = PlayerDataBase.instance.GetPartyCri(partyNumber, rare);

        int critical;
        
        if(cri >= 100)
        {
            critical = 2;
        }
        else
        {
            if (Random.Range(0f, 100f) <= cri)   critical = 2;
            else critical = 1;
        }

        if (rare)
        {
            switch(num)
            {
                case 0:
                    battle.enemyCurrentHP -= atk * 2 * critical;
                    damage.SetActive(false);
                    damage.SetActive(true);
                    damage.transform.position = enemy.gameObject.transform.position;
                    damageFont.text = "-" + atk * 2 * critical;
                    damageFont.color = Color.red;
                    battle.HPBarChange(false);
                    enemy.Stun(5f);
                    break;
            }
        }
        else
        {
            switch (num)
            {
                case 0:
                    battle.enemyCurrentHP -= (int)(atk * 1.2f * critical);
                    damage.SetActive(false);
                    damage.SetActive(true);
                    damage.transform.position = enemy.gameObject.transform.position;
                    damageFont.text = "-" + (int)(atk * 1.2f * critical);
                    damageFont.color = Color.red;
                    battle.HPBarChange(false);
                    bp[partyNumber].TargetOn(5f);
                    enemy.TarggettingOn(partyNumber, 5f);
                    break;
                case 1:
                    battle.playerCurrentHP += atk * critical;
                    damage.SetActive(false);
                    damage.SetActive(true);
                    damage.transform.position = partyBar.position;
                    damageFont.text = "+" + atk * critical;
                    damageFont.color = Color.green;
                    if (battle.playerCurrentHP > battle.playerMaxHP)
                    {
                        battle.playerCurrentHP = battle.playerMaxHP;
                    }
                    battle.HPBarChange(true);
                    break;
            }
        }


        battle.EffectOn(partyNumber, false);
    }
}
