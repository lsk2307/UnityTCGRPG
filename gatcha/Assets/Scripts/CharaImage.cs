using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharaImage : MonoBehaviour, IPointerClickHandler
{
    GameObject playerInfo;
    UIManager uim;
    
    public GameObject indexObj;
    public Image charaImg;
    public Image[] starImg;
    public Sprite[] starSprites;
    public Text levelText;
    public GameObject partyIcon;

    public Text rareText;

    bool playerRare;
    bool inParty;
    int playerLevel;
    int playerStar;
    int playerNum;

    int invenIndex;

    void Awake()
    {
        //charaImg = GetComponent<Image>();
        uim = GameObject.Find("UI").GetComponent<UIManager>();
        indexObj = GameObject.Find("UI").transform.Find("CharaIndex").gameObject;
    }

    //이미지 셋팅
    public void SetImg(int num, int star, int level, int index, bool party, bool rare)
    {
        if(rare)
        {
            charaImg.sprite = PlayerDataBase.instance.PlayerSRare[num].sprites[star / 3];
        }
        else
        {
            charaImg.sprite = PlayerDataBase.instance.PlayerRare[num].sprites[star / 3];
        }

        playerRare = rare;

        inParty = party;

        if(inParty)
        {
            partyIcon.SetActive(true);
        }

        for (int i = 0; i < star; ++i)
        {
            starImg[i].sprite = starSprites[1];
        }

        for (int i = star; i < 5; ++i)
        {
            starImg[i].sprite = starSprites[0];
        }

        playerLevel = level;
        playerStar = star;
        playerNum = num;
        invenIndex = index;

        levelText.text = level.ToString();

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
    }

    //캐릭터 정보 창 켜기
    public void CharaInfoOn()
    {

        GameObject.Find("UI").transform.Find("CharaInfo").gameObject.SetActive(true);

        playerInfo = GameObject.Find("CharaInfo");

        CharaInfo ci = playerInfo.GetComponent<CharaInfo>();

        ci.SetInfo(charaImg.sprite, playerStar, playerLevel, playerNum, invenIndex,inParty, playerRare);

        if(inParty)
        {
            uim.PartyOff();
        }
        else if(indexObj.gameObject.activeSelf)
        {
            uim.CharaIndexOff(0);

            ci.onIndex = 0;
        }
        else
        {
            uim.CharaIndexOff(1);
            ci.onIndex = 1;
        }
       // index.SetActive(false);
    }

    //슬라이더와 클릭이 겹치지 않기위해 따로 클릭설정
    public void OnPointerClick(PointerEventData eventData)
    {
        CharaInfoOn();
    }

}
