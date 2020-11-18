using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject charaIndex;
    public GameObject gatcha;
    public GameObject charaInfo;
    public GameObject partyIndex;
    public GameObject cashBuy;
    public CharaInven charaInven;
    public Party party;
    public Text cash;
    public Text gold;

    private void Awake()
    {
        if(cash != null)
        cash.text = PlayerDataBase.instance.cash.ToString();
        if(gold != null)
        gold.text = PlayerDataBase.instance.gold.ToString();
    }

    //캐릭터 목록 켜기
    public void CharaIndexOn()
    {
        BuyCahsNo();
        gatcha.SetActive(false);
        charaInfo.SetActive(false);
        partyIndex.SetActive(false);

        charaIndex.SetActive(true);
        for (int i = 0; i < PlayerDataBase.instance.PlayerIndex.Count; ++i)
        {
            charaInven.chara[i].SetActive(true);

            CharaImage ci = charaInven.chara[i].GetComponent<CharaImage>();
            ci.SetImg(PlayerDataBase.instance.PlayerIndex[i].playerNum, PlayerDataBase.instance.PlayerIndex[i].playerStar, PlayerDataBase.instance.PlayerIndex[i].playerLevel, i, false, PlayerDataBase.instance.PlayerIndex[i].playerRare);
        }
    }

    //캐릭터 목록 끄기
    public void CharaIndexOff(int num)
    {
        switch (num)
        {
            case 0:
                charaIndex.SetActive(false);

                for (int i = 0; i < PlayerDataBase.instance.PlayerIndex.Count; ++i)
                {
                    charaInven.chara[i].SetActive(false);
                }
                break;
            case 1:
                partyIndex.SetActive(false);

                for (int i = 0; i < PlayerDataBase.instance.PlayerIndex.Count; ++i)
                {
                    party.chara[i].SetActive(false);
                }
                break;
        }
    }

    //가챠 창 켜기
    public void GatchaOn()
    {
        BuyCahsNo();
        charaIndex.SetActive(false);
        charaInfo.SetActive(false);
        partyIndex.SetActive(false);

        gatcha.SetActive(true);
    }

    //파티 설정 창 켜기
    public void PartyOn()
    {
        BuyCahsNo();
        charaIndex.SetActive(false);
        gatcha.SetActive(false);
        charaInfo.SetActive(false);

        partyIndex.SetActive(true);

        for (int i = 0; i < PlayerDataBase.instance.PlayerIndex.Count; ++i)
        {
            party.chara[i].SetActive(true);

            CharaImage ci = party.chara[i].GetComponent<CharaImage>();
            ci.SetImg(PlayerDataBase.instance.PlayerIndex[i].playerNum, PlayerDataBase.instance.PlayerIndex[i].playerStar, PlayerDataBase.instance.PlayerIndex[i].playerLevel, i, false, PlayerDataBase.instance.PlayerIndex[i].playerRare);
        }
    }

    //파티 설정 창 끄기
    public void PartyOff()
    {
        partyIndex.SetActive(false);
    }

    //게임 종료
    public void GameOff()
    {
        Application.Quit();
    }

    //스테이지 씬으로
    public void LoadStageScene()
    {
        SceneManager.LoadScene(1);
    }

    //메인 씬으로
    public void MainStageScene()
    {
        SceneManager.LoadScene(0);
    }

    public void CashRefresh()
    {
        cash.text = PlayerDataBase.instance.cash.ToString();
    }

    public void GoldRefresh()
    {
        gold.text = PlayerDataBase.instance.gold.ToString();
    }

    public void BuyCash()
    {
        cashBuy.SetActive(true);
    }

    public void BuyCashYes()
    {
        if (PlayerDataBase.instance.gold < 500) return; 

        PlayerDataBase.instance.GetCash(100);
        PlayerDataBase.instance.GetGold(-500);

        CashRefresh();
        GoldRefresh();

        cashBuy.SetActive(false);
    }

    public void BuyCahsNo()
    {
        cashBuy.SetActive(false);
    }
}
