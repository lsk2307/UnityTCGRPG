using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class Battle : MonoBehaviour
{
    public GameObject[] party;
    public Image[] partyImages;
    public Text[] partySkills;
    public Text[] HP; //0 플레이어 1 에너미
    public Slider[] HPBar;

    public GameObject skillBar;
    public GameObject end;
    public GameObject enemyEffect;
    public GameObject partyEffect;
    public Enemy enemy;
    public BattleParty[] bp;

    public GameObject damage;
    public Text damageFont;

    public Text endText;
    public Text messege;

    public List<GameObject> effect;

    public int playerMaxHP;
    public int playerCurrentHP;
    public int playerDef;
    public int enemyMaxHP;
    public int enemyCurrentHP;

    bool endding;
    float attackCool;

    private void Awake()
    {
        PlayerDataBase.instance.PlayerHPSet();

        playerMaxHP = PlayerDataBase.instance.partyHP;
        playerCurrentHP = playerMaxHP;
        playerDef = PlayerDataBase.instance.partyDef;
        enemyMaxHP = enemy.bossHP;
        enemyCurrentHP = enemyMaxHP;

        HPBarChange(true);
        HPBarChange(false);

        skillBar.SetActive(false);

        for (int i = 0; i < PlayerDataBase.instance.PartyIndex.Count; ++i)
        {
            party[i].SetActive(true);

            if (PlayerDataBase.instance.PartyIndex[i].playerRare == true)
            {
                if(PlayerDataBase.instance.PartyIndex[i].playerStar >= 3)
                {
                    partyImages[i].sprite = PlayerDataBase.instance.PlayerSRare[PlayerDataBase.instance.PartyIndex[i].playerNum].sprites[1];
                }
                else
                {
                    partyImages[i].sprite = PlayerDataBase.instance.PlayerSRare[PlayerDataBase.instance.PartyIndex[i].playerNum].sprites[0];
                }

                string str = "";
                foreach (var ar in PlayerDataBase.instance.skillInfoSRare[PlayerDataBase.instance.PartyIndex[i].playerNum])
                {
                    str += ar;
                }

                string[] sp = str.Split(':');

                partySkills[i].text = sp[0];

            }
            else
            {
                if (PlayerDataBase.instance.PartyIndex[i].playerStar >= 3)
                {
                    partyImages[i].sprite = PlayerDataBase.instance.PlayerRare[PlayerDataBase.instance.PartyIndex[i].playerNum].sprites[1];
                }
                else
                {
                    partyImages[i].sprite = PlayerDataBase.instance.PlayerRare[PlayerDataBase.instance.PartyIndex[i].playerNum].sprites[0];
                }

                string str = "";
                foreach (var ar in PlayerDataBase.instance.skillInfoRare[PlayerDataBase.instance.PartyIndex[i].playerNum])
                {
                    str += ar;
                }

                string[] sp = str.Split(':');

                partySkills[i].text = sp[0];
            }
        }

        bool rare;
        int num;

        string effectName;
        GameObject skillPrefab;
        for (int i = 0; i < PlayerDataBase.instance.PartyIndex.Count; ++i)
        {
            rare = PlayerDataBase.instance.PartyIndex[i].playerRare;
            num = PlayerDataBase.instance.PartyIndex[i].playerNum;


            if (rare)
            {
                effectName = "SR" + num;
            }
            else
            {
                effectName = "R" + num;
            }

            if (effectName == "R1")
            { 
                skillPrefab = Instantiate(Resources.Load("Prefabs/Effect/" + effectName), partyEffect.transform) as GameObject;
                skillPrefab.transform.parent = partyEffect.transform;
            }
            else
            {
                skillPrefab = Instantiate(Resources.Load("Prefabs/Effect/" + effectName), enemyEffect.transform) as GameObject;
                skillPrefab.transform.parent = enemyEffect.transform;
            }

            effect.Add(skillPrefab);
        }

        effectName = "MeleeAttack";
        skillPrefab = Instantiate(Resources.Load("Prefabs/Effect/" + effectName), enemyEffect.transform) as GameObject;
        skillPrefab.transform.parent = enemyEffect.transform;

        effect.Add(skillPrefab);

        effectName = "E0";
        skillPrefab = Instantiate(Resources.Load("Prefabs/Effect/" + effectName), partyEffect.transform) as GameObject;
        skillPrefab.transform.parent = partyEffect.transform;

        effect.Add(skillPrefab);
    }

    private void Update()
    {
        if (!endding)
        {
            if (enemyCurrentHP <= 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (!bp[i].enabled) break;
                    bp[i].endding = true;
                }

                endding = true;
                enemy.endding = true;
                end.SetActive(true);
                endText.text = "승리";
                PlayerDataBase.instance.GetGold(500);
                messege.text = "상대를 제압하여 500G를 획득하였다. 메인으로 돌아갑니다.";
            }
            else if (playerCurrentHP <= 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (!bp[i].enabled) break;
                    bp[i].endding = true;
                }
                enemy.endding = true;
                endding = true;
                end.SetActive(true);
                endText.text = "패배";
                messege.text = "상대에게 제압당해버렸다. 메인으로 돌아갑니다.";
            }

            attackCool += Time.deltaTime;

            if(attackCool >= 3f)
            {
                int atk = PlayerDataBase.instance.GetPartyAllAtk();

                enemyCurrentHP -= atk;
                HPBarChange(false);
                attackCool = 0;
                effect[effect.Count - 2].SetActive(true);
                damage.SetActive(true);
                damage.transform.position = enemy.gameObject.transform.position;
                damageFont.text = "-" + atk;
                damageFont.color = Color.red;
            }
        }
    }

    public void BattleOut()
    {
        for (int i = 0; i < 4; ++i)
        {
            party[i].SetActive(false);
        }
        SceneManager.LoadScene(0);
    }

    public void HPBarChange(bool player)
    {
        if(player)
        {
            HP[0].text = "PlayerHP : " + playerCurrentHP + " / " + playerMaxHP;
            HPBar[0].value = (float)playerCurrentHP / (float)playerMaxHP;
        }
        else
        {
            HP[1].text = "EnemyHP : " + enemyCurrentHP + " / " + enemyMaxHP;
            HPBar[1].value = (float)enemyCurrentHP / (float)enemyMaxHP;
        }

    }

    public void EffectOn(int num, bool boss)
    {
        if(boss)
        {
            effect[effect.Count-1].SetActive(true);
        }
        else
        {
            effect[num].SetActive(true);
        }
    }
}
