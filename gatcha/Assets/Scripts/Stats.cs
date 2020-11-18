using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats
{
    //캐릭터 기본 정보
    public int playerNum;
    public string name;
    public Sprite[] sprites;
    public int maxHP;
    public int currentHP;
    public int damage;
    public int def;
    public float cri;
    public int level;
    public int starLevel;

    //레벨업당 증가요소
    public int levelUpHP;
    public int levelUpDamage;
    public int levelUpDef;
    public float levelUpCri;

    //별 업 당 증가요소
    public int starUpHP;
    public int starUpDamage;
    public int starUpDef;
    public float starUpCri;

    //레어리티
    public int rare;

    //파티에 들어가있는지 체크
    public bool inParty;

    //캐릭터 현재 이미지
    Image sprite;

    public Stats(int PlayerNum, string PlayerName, Sprite[] playerSprites, int MaxHP, int CurrentHP, int Damage, int Def, float Cri, int Level, int StarLevel, int LevelUpHp,
                  int LevelUpDamage, int LevelUpDef, float LevelUpCri, int StarUpHp, int StarUpDamage, int StarUpDef, float StarUpCri, int Rare, bool party)
    {
        playerNum = PlayerNum;
        name = PlayerName;
        sprites = playerSprites;
        maxHP = MaxHP;
        currentHP = CurrentHP;
        damage = Damage;
        def = Def;
        cri = Cri;
        level = Level;
        starLevel = StarLevel;

        levelUpHP = LevelUpHp;
        levelUpDamage = LevelUpDamage;
        levelUpDef = LevelUpDef;
        levelUpCri = LevelUpCri;

        starUpHP = StarUpHp;
        starUpDamage = StarUpDamage;
        starUpDef = StarUpDef;
        starUpCri = StarUpCri;

        rare = Rare;
        inParty = party;
    }

}
