using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public GameObject p;
    public List<GameObject> chara;
    public GameObject[] partyChara;
    public Text[] stats;

    RectTransform rect;

    bool playerRare;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        //파티 목록 최대 갯수
        for (int i = 0; i < 50; i++)
        {
            GameObject gob = Instantiate(p) as GameObject;
            gob.transform.SetParent(GameObject.Find("Content").transform);
            gob.transform.localScale = new Vector3(1, 1, 1);
            gob.SetActive(false);

            chara.Add(gob);
        }
    }


    //파티 UI 설정
    private void OnEnable()
    {
        for (int i = 0; i < 4; ++i)
        {
            partyChara[i].SetActive(false);
        }

        //int hp = 0;
        //int def = 0;

        for (int i = 0; i < PlayerDataBase.instance.PartyIndex.Count; ++i)
        {
            partyChara[i].SetActive(true);

            CharaImage ci = partyChara[i].GetComponent<CharaImage>();

            ci.SetImg(PlayerDataBase.instance.PartyIndex[i].playerNum, PlayerDataBase.instance.PartyIndex[i].playerStar, PlayerDataBase.instance.PartyIndex[i].playerLevel, i, true, PlayerDataBase.instance.PartyIndex[i].playerRare);

        }

        PlayerDataBase.instance.PlayerHPSet();

        stats[0].text = PlayerDataBase.instance.partyHP.ToString();
        stats[1].text = PlayerDataBase.instance.partyDef.ToString();

        rect.localPosition = new Vector3(250 * PlayerDataBase.instance.PlayerIndex.Count, 0, 0);

    }
}
