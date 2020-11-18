using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaInven : MonoBehaviour
{

    public GameObject p;

    RectTransform rect;

    public List<GameObject> chara;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        //캐릭터 목록 최대 갯수
        for (int i = 0; i < 50; i++)
        {
            GameObject gob = Instantiate(p) as GameObject;
            gob.transform.SetParent(GameObject.Find("Content").transform);
            gob.transform.localScale = new Vector3(1, 1, 1);
            gob.SetActive(false);

            chara.Add(gob);
        }
    }


    private void OnEnable()
    {
        //위치 초기화
        rect.localPosition = new Vector3(0, 0, 0);
    }




}
