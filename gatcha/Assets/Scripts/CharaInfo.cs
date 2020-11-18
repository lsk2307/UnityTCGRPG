using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaInfo : MonoBehaviour
{
    public UIManager uiM;
    //0 - name, 1 - level, 2 - hp, 3 - atk, 4 - def, 5 - cri
    public Text[] infoText;
    public Text rareText;
    public Image playerImage;
    public Image[] starImages;
    public Sprite[] starSprites;
    public GameObject sellMessage;
    public GameObject levelUpMessage;
    public GameObject starUpMessage;
    public GameObject skillTab;
    public Image partyImage;
    public Sprite[] partySprite;

    public Text skillText;
    public Text sellText;
    public Text goldText;
    public Text levelUpText;
    public Text starUpText;

    public int onIndex;

    int invenIndex;
    int playerLevel;
    int playerNum;
    int playerStar;
    bool inParty;
    bool playeRare;

    //캐릭터 정보 셋팅
    public void SetInfo(Sprite image, int star, int level, int num, int index, bool party, bool rare)
    {
        inParty = party;
        invenIndex = index;
        playerNum = num;
        playerLevel = level;
        playerImage.sprite = image;
        playerStar = star;
        playeRare = rare;

        string name;

        if (rare)
        {
            name = PlayerDataBase.instance.PlayerSRare[num].name;
        }
        else
        {
            name = PlayerDataBase.instance.PlayerRare[num].name;
        }

        infoText[0].text = name;

        Setting();

        Debug.Log(name);

        if (!rare)
        {
            rareText.color = Color.red;
            rareText.text = "R";
        }
        else
        {
            rareText.color = Color.yellow;
            rareText.text = "SR";
        }

        if (inParty)
        {
            partyImage.sprite = partySprite[1];
        }
        else
        {
            partyImage.sprite = partySprite[0];
        }

    }

    //캐릭터 정보 끄기
    public void SetInfoOff()
    {
        if (inParty)
        {
            uiM.PartyOn();
        }
        else
        {
            switch (onIndex)
            {
                case 0:
                    uiM.CharaIndexOn();
                    break;
                case 1:

                    uiM.PartyOn();
                    break;
            }
        }
       
        gameObject.SetActive(false);
    }

    //캐릭터 매각 창 켜기
    public void CharaSell()
    {
        if (inParty) return;
        sellMessage.SetActive(true);
        sellText.text = "파시면 돌려 받을 수 없습니다 정말 파시겠습니까? (" + (int)(PlayerDataBase.instance.PlayerIndex[invenIndex].playerStar * PlayerDataBase.instance.PlayerIndex[invenIndex].playerLevel * (PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare == true ? 10 : 5)) + " Gold 획득)";
    }

    //캐릭터 매각 Yes
    public void CharaSellYes()
    {
        if(PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare)
        {
            PlayerDataBase.instance.gold += PlayerDataBase.instance.PlayerIndex[invenIndex].playerStar * PlayerDataBase.instance.PlayerIndex[invenIndex].playerLevel * 10;
        }
        else
        {
            PlayerDataBase.instance.gold += PlayerDataBase.instance.PlayerIndex[invenIndex].playerStar * PlayerDataBase.instance.PlayerIndex[invenIndex].playerLevel * 5;
        }

        goldText.text = PlayerDataBase.instance.gold.ToString();
        PlayerDataBase.instance.PlayerIndex.RemoveAt(invenIndex);
        sellMessage.SetActive(false);
        if (inParty)
        {
            uiM.PartyOn();
        }
        else
        {
            switch (onIndex)
            {
                case 0:
                    uiM.CharaIndexOn();
                    break;
                case 1:

                    uiM.PartyOn();
                    break;
            }
        }
        gameObject.SetActive(false);
    }

    //캐릭터 매각 No
    public void CharaSellNo()
    {
        sellMessage.SetActive(false);
    }

    //별올리기
    public void StarUp()
    {
        if (playerStar == 5) return;

        starUpMessage.SetActive(true);

        int pay; 

        if (inParty)
        {
            pay = PlayerDataBase.instance.PartyIndex[invenIndex].playerStar * (PlayerDataBase.instance.PartyIndex[invenIndex].playerRare == true ? 100 : 50);
        }
        else
        {
            pay = PlayerDataBase.instance.PlayerIndex[invenIndex].playerStar * (PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare == true ? 100 : 50);
        }
        starUpText.text = "스타 업 시키는데 드는 비용 (" + pay + " Gold 지불)";
    }

    public void StarUpYes()
    {
        int pay;

        if (inParty)
        {
            pay = PlayerDataBase.instance.PartyIndex[invenIndex].playerStar * (PlayerDataBase.instance.PartyIndex[invenIndex].playerRare == true ? 100 : 50);
        }
        else
        {
            pay = PlayerDataBase.instance.PlayerIndex[invenIndex].playerStar * (PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare == true ? 100 : 50);
        }

        if (PlayerDataBase.instance.gold < pay) return;

        PlayerDataBase.instance.GetGold(-1 * pay);

        if (playerStar == 2)
        {
            if (playeRare)
            {
                playerImage.sprite = PlayerDataBase.instance.PlayerSRare[playerNum].sprites[1];
            }
            else
            {
                playerImage.sprite = PlayerDataBase.instance.PlayerRare[playerNum].sprites[1];
            }
        }

        PlayerDataBase.instance.StarUp(invenIndex, inParty);
        playerStar++;
        Setting();

        starUpMessage.SetActive(false);
    }

    public void StarUpNo()
    {
        starUpMessage.SetActive(false);
    }

    //레벨 올리기
    public void LevelUp()
    {
        levelUpMessage.SetActive(true);

        int pay;

        if (inParty)
        {
            pay = PlayerDataBase.instance.PartyIndex[invenIndex].playerLevel * (PlayerDataBase.instance.PartyIndex[invenIndex].playerRare == true ? 10 : 5);
        }
        else
        {
            pay = PlayerDataBase.instance.PlayerIndex[invenIndex].playerLevel * (PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare == true ? 10 : 5);
        }

        levelUpText.text = "레벨 업 시키는데 드는 비용 (" + pay + " Gold 지불)";
    }

    public void LevelUpYes()
    {
        int pay;

        if (inParty)
        {
            pay = PlayerDataBase.instance.PartyIndex[invenIndex].playerLevel * (PlayerDataBase.instance.PartyIndex[invenIndex].playerRare == true ? 10 : 5);
        }
        else
        {
            pay = PlayerDataBase.instance.PlayerIndex[invenIndex].playerLevel * (PlayerDataBase.instance.PlayerIndex[invenIndex].playerRare == true ? 10 : 5);
        }

        if (PlayerDataBase.instance.gold < pay) return;

        PlayerDataBase.instance.GetGold(-1 * pay);

        PlayerDataBase.instance.LevelUp(invenIndex, inParty);
        playerLevel++;
        Setting();

        levelUpMessage.SetActive(false);
    }

    public void LevelUpNo()
    {
        levelUpMessage.SetActive(false);
    }


    //정보 셋팅
    void Setting()
    {
        infoText[1].text = playerLevel.ToString();

        if(playeRare)
        {
            int hp = PlayerDataBase.instance.PlayerSRare[playerNum].maxHP + PlayerDataBase.instance.PlayerSRare[playerNum].levelUpHP * (playerLevel - 1) + PlayerDataBase.instance.PlayerSRare[playerNum].starUpHP * (playerStar - 1);
            int atk = PlayerDataBase.instance.PlayerSRare[playerNum].damage + PlayerDataBase.instance.PlayerSRare[playerNum].levelUpDamage * (playerLevel - 1) + PlayerDataBase.instance.PlayerSRare[playerNum].starUpDamage * (playerStar - 1);
            int def = PlayerDataBase.instance.PlayerSRare[playerNum].def + PlayerDataBase.instance.PlayerSRare[playerNum].levelUpDef * (playerLevel - 1) + PlayerDataBase.instance.PlayerSRare[playerNum].starUpDef * (playerStar - 1);
            float cri = PlayerDataBase.instance.PlayerSRare[playerNum].cri + PlayerDataBase.instance.PlayerSRare[playerNum].levelUpCri * (playerLevel - 1) + PlayerDataBase.instance.PlayerSRare[playerNum].starUpCri * (playerStar - 1);


            infoText[2].text = hp.ToString();
            infoText[3].text = atk.ToString();
            infoText[4].text = def.ToString();
            infoText[5].text = cri.ToString();

        }
        else
        {
            int hp = PlayerDataBase.instance.PlayerRare[playerNum].maxHP + PlayerDataBase.instance.PlayerRare[playerNum].levelUpHP * (playerLevel - 1) + PlayerDataBase.instance.PlayerRare[playerNum].starUpHP * (playerStar - 1);
            int atk = PlayerDataBase.instance.PlayerRare[playerNum].damage + PlayerDataBase.instance.PlayerRare[playerNum].levelUpDamage * (playerLevel - 1) + PlayerDataBase.instance.PlayerRare[playerNum].starUpDamage * (playerStar - 1);
            int def = PlayerDataBase.instance.PlayerRare[playerNum].def + PlayerDataBase.instance.PlayerRare[playerNum].levelUpDef * (playerLevel - 1) + PlayerDataBase.instance.PlayerRare[playerNum].starUpDef * (playerStar - 1);
            float cri = PlayerDataBase.instance.PlayerRare[playerNum].cri + PlayerDataBase.instance.PlayerRare[playerNum].levelUpCri * (playerLevel - 1) + PlayerDataBase.instance.PlayerRare[playerNum].starUpCri * (playerStar - 1);


            infoText[2].text = hp.ToString();
            infoText[3].text = atk.ToString();
            infoText[4].text = def.ToString();
            infoText[5].text = cri.ToString();

        }

        for (int i = 0; i < playerStar; ++i)
        {
            starImages[i].sprite = starSprites[1];
        }

        for (int i = playerStar; i < 5; ++i)
        {
            starImages[i].sprite = starSprites[0];
        }

        uiM.GoldRefresh();
    }

    //파티 버튼 클릭시
    public void PartySet()
    {
        //파티에 들어가 있으면 넣고 들어가 있지 않으면 빼기
       if(inParty)
       {
            PlayerDataBase.instance.PartyOut(invenIndex);
            PlayerDataBase.instance.PlayerIndex.Add(PlayerDataBase.instance.PartyIndex[invenIndex]);
            PlayerDataBase.instance.PartyIndex.RemoveAt(invenIndex);

            uiM.PartyOn();
            gameObject.SetActive(false);
       }
       else
       {
            if (PlayerDataBase.instance.PartyIndex.Count >= 4) return;

            PlayerDataBase.instance.PartyIn(invenIndex);
            PlayerDataBase.instance.PartyIndex.Add(PlayerDataBase.instance.PlayerIndex[invenIndex]);
            PlayerDataBase.instance.PlayerIndex.RemoveAt(invenIndex);

            uiM.PartyOn();
            gameObject.SetActive(false);
       }
    }

    //스킬 정보창 켜기
    public void SkillInfo()
    {
        skillTab.SetActive(true);

        string a = "";
        if (playeRare)
        {
            foreach (var ar in PlayerDataBase.instance.skillInfoSRare[playerNum])
            {
                a += ar;
            }
        }
        else
        {
            foreach (var ar in PlayerDataBase.instance.skillInfoRare[playerNum])
            {
                a += ar;
            }
        }
        

        skillText.text = a;
    }

    //스킬 정보창 끄기
    public void SkillOff()
    {
        skillTab.SetActive(false);
    }

    //정보창 꺼질때 잡다한 창 꺼짐
    private void OnDisable()
    {
        SkillOff();
        CharaSellNo();
        StarUpNo();
        LevelUpNo();
    }
}
