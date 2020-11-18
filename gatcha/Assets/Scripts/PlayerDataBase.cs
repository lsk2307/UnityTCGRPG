using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerDataBase : MonoBehaviour
{
    //public static PlayerDataBase Instance;

    [System.Serializable]
    public struct PlayerStats
    {
        public int playerNum;
        public int playerLevel;
        public int playerStar;
        public bool inParty;
        public bool playerRare;
    };

    public static PlayerDataBase instance;
    //캐릭터 정보
    public List<Stats> PlayerRare = new List<Stats>();
    public List<Stats> PlayerSRare = new List<Stats>();
    //캐릭터 목록
    public List<PlayerStats> PlayerIndex = new List<PlayerStats>();
    //파티 목록
    public List<PlayerStats> PartyIndex = new List<PlayerStats>();

    //플레이어 재화
    public int gold;
    public int cash;

    //스킬 설명
    public List<string[]> skillInfoRare = new List<string[]>();
    public List<string[]> skillInfoSRare = new List<string[]>();

    public int partyHP;
    public int partyDef;

    public int stageNum;

    private void Awake()
    {
        //오브젝트 중복 확인
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);


        //저장된 데이터 로드
        Load();
        //스킬정보 로드
        SetSkillInfo();
    }

    //캐릭터 정보 추가
    void RareAdd(int PlayerNum, string PlayerName, int MaxHP, int CurrentHP, int Damage, int Def, float Cri, int Level, int StarLevel, int LevelUpHp,
                  int LevelUpDamage, int LevelUpDef, float LevelUpCri, int StarUpHp, int StarUpDamage, int StarUpDef, float StarUpCri, int Rare, bool party)
    {
        PlayerRare.Add(new Stats(PlayerNum,PlayerName, Resources.LoadAll<Sprite>("Images/Player/R/" + PlayerNum), MaxHP,  CurrentHP,  Damage,  Def,  Cri,  Level,  StarLevel,  LevelUpHp,
                   LevelUpDamage,  LevelUpDef,  LevelUpCri,  StarUpHp,  StarUpDamage,  StarUpDef,  StarUpCri, Rare, party));
    }

    void SRareAdd(int PlayerNum, string PlayerName, int MaxHP, int CurrentHP, int Damage, int Def, float Cri, int Level, int StarLevel, int LevelUpHp,
                  int LevelUpDamage, int LevelUpDef, float LevelUpCri, int StarUpHp, int StarUpDamage, int StarUpDef, float StarUpCri, int Rare, bool party)
    {
        PlayerSRare.Add(new Stats(PlayerNum, PlayerName, Resources.LoadAll<Sprite>("Images/Player/SR/" + PlayerNum), MaxHP, CurrentHP, Damage, Def, Cri, Level, StarLevel, LevelUpHp,
                   LevelUpDamage, LevelUpDef, LevelUpCri, StarUpHp, StarUpDamage, StarUpDef, StarUpCri, Rare, party));
    }

    private void Start()
    {
        //캐릭터 정보 추가
        RareAdd(0, "레나", 100, 100, 20, 0, 0, 1, 1, 50, 5, 1, 0.1f, 150, 50, 10, 5f, 0, false);
        RareAdd(1, "소나", 80, 80, 10, 0, 0, 1, 1, 30, 3, 1, 0.05f, 100, 30, 5, 3f, 0, false);

        SRareAdd(0, "타냐", 80, 80, 30, 0, 0, 1, 2, 30, 8, 1, 0.15f, 100, 70, 5, 10f, 1, false);

    }

    //캐릭터목록 추가
    public void CharaGet(int num, bool rare)
    {
        PlayerStats ps = new PlayerStats();

        ps.playerNum = num;
        ps.playerLevel = 1;
        ps.playerRare = rare;

        if(rare)
        {
            ps.playerStar = PlayerSRare[num].starLevel;
        }
        else
        {
            ps.playerStar = PlayerRare[num].starLevel;
        }

        PlayerIndex.Add(ps);
    }

    //리스트 저장을 위해(json)
    [System.Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    //저장
    public void Save()
    {
        string toJson = JsonUtility.ToJson(new Serialization<PlayerStats> (PlayerIndex));
        //Debug.Log(toJson);
        if (Application.platform == RuntimePlatform.Android)
        {
            File.WriteAllText(Application.persistentDataPath + "/data.json", toJson);
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/Saves/data.json", toJson);
        }

        string partyJson = JsonUtility.ToJson(new Serialization<PlayerStats>(PartyIndex));

        if (Application.platform == RuntimePlatform.Android)
        {
            File.WriteAllText(Application.persistentDataPath + "/partyData.json", partyJson);
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/Saves/partyData.json", partyJson);
        }
        Debug.Log("저장");

        List<int> playerInfo = new List<int>();

        playerInfo.Add(gold);
        playerInfo.Add(cash);

        string playerInfoJson = JsonUtility.ToJson(new Serialization<int>(playerInfo));

        if (Application.platform == RuntimePlatform.Android)
        {
            File.WriteAllText(Application.persistentDataPath + "/playerInfo.json", playerInfoJson);
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/Saves/playerInfo.json", playerInfoJson);
        }
    }

    //불러오기
    public void Load()
    {

        string strFile;
        if (Application.platform == RuntimePlatform.Android)
        {
            strFile = Application.persistentDataPath + "/data.json";
        }
        else
        {
            strFile = "Assets/Saves/data.json";
        }

        FileInfo fileInfo = new FileInfo(strFile);
        //파일 있는지 확인 있을때(true), 없으면(false)
        if (!fileInfo.Exists)
        {
            Debug.Log("파일없음");
            return;
        }

        string jsonString;

        if (Application.platform == RuntimePlatform.Android)
        {
            jsonString = File.ReadAllText(Application.persistentDataPath + "/data.json");
        }
        else
        {
            jsonString = File.ReadAllText(Application.dataPath + "/Saves/data.json");
        }

        PlayerIndex = JsonUtility.FromJson<Serialization<PlayerStats>>(jsonString).ToList();

        string strPartyFile;
        if (Application.platform == RuntimePlatform.Android)
        {
            strPartyFile = Application.persistentDataPath + "/partyData.json";
        }
        else
        {

            strPartyFile = "Assets/Saves/partyData.json";
        }

        FileInfo partyFileInfo = new FileInfo(strPartyFile);
        //파일 있는지 확인 있을때(true), 없으면(false)
        if (!partyFileInfo.Exists)
        {
            Debug.Log("파일없음");
            return;
        }


        string partyJsonString;
        if (Application.platform == RuntimePlatform.Android)
        {
            partyJsonString = File.ReadAllText(Application.persistentDataPath + "/partyData.json");
        }
        else
        {
            partyJsonString = File.ReadAllText(Application.dataPath + "/Saves/partyData.json");
        }

        PartyIndex = JsonUtility.FromJson<Serialization<PlayerStats>>(partyJsonString).ToList();

        Debug.Log("불러오기");


        string strPlayerInfoFile;
        if (Application.platform == RuntimePlatform.Android)
        {
            strPlayerInfoFile = Application.persistentDataPath + "/playerInfo.json";
        }
        else
        {
            strPlayerInfoFile = "Assets/Saves/playerInfo.json";
        }

        FileInfo playerInfoFileInfo = new FileInfo(strPlayerInfoFile);
        //파일 있는지 확인 있을때(true), 없으면(false)
        if (!playerInfoFileInfo.Exists)
        {
            Debug.Log("파일없음");
            return;
        }


        string playerInfoJsonString;
        if (Application.platform == RuntimePlatform.Android)
        {
            playerInfoJsonString = File.ReadAllText(Application.persistentDataPath + "/playerInfo.json");
        }
        else
        {
            playerInfoJsonString = File.ReadAllText(Application.dataPath + "/Saves/playerInfo.json");
        }

        List<int> playerInfo = new List<int>();

        playerInfo = JsonUtility.FromJson<Serialization<int>>(playerInfoJsonString).ToList();

        gold = playerInfo[0];
        cash = playerInfo[1];
    }

    //캐릭터 별올리기
    public void StarUp(int num, bool party)
    {
        if(!party)
        {
            PlayerStats ps = PlayerIndex[num];
            ps.playerStar++;

            PlayerIndex[num] = ps;
        }
        else
        {
            PlayerStats ps = PartyIndex[num];
            ps.playerStar++;

            PartyIndex[num] = ps;
        }
    }


    //캐릭터 레벨 올리기
    public void LevelUp(int num, bool party)
    {
        if (!party)
        {
            PlayerStats ps = PlayerIndex[num];
            ps.playerLevel++;

            PlayerIndex[num] = ps;
        }
        else
        {
            PlayerStats ps = PartyIndex[num];
            ps.playerLevel++;

            PartyIndex[num] = ps;
        }
    }

    //파티목록에 추가
    public void PartyIn(int num)
    {
            PlayerStats ps = PlayerIndex[num];
            ps.inParty = true;

            PlayerIndex[num] = ps;
    }

    //파티목록에서 빼기
    public void PartyOut(int num)
    {
            PlayerStats ps = PartyIndex[num];
            ps.inParty = false;

            PartyIndex[num] = ps;
    }

    //어플 종료와 함께 데이터 세이브
    private void OnApplicationQuit()
    {
        Save();
    }


    //스킬정보 불러오기(txt)
    void SetSkillInfo()
    {
        TextAsset txt = Resources.Load("Data/skillDataRare",typeof(TextAsset)) as TextAsset;
        StringReader reader = new StringReader(txt.text);
        

        while(true)
        {
            string line = reader.ReadLine();

            if (line == null)
                break;
            
            string[] arr = line.Split(';');

            skillInfoRare.Add(arr);
        }

        txt = Resources.Load("Data/skillDataSRare", typeof(TextAsset)) as TextAsset;
        reader = new StringReader(txt.text);


        while (true)
        {
            string line = reader.ReadLine();

            if (line == null)
                break;

            string[] arr = line.Split(';');

            skillInfoSRare.Add(arr);
        }

    }

    public void SkillUse(int num, bool rare)
    {
        if(rare)
        {
            switch (num)
            {
                //썬더2
                case 0:
                    break;
            }
        }
        else
        {
            switch (num)
            {
                //강타1
                case 0:
                    break;
                //풀힐1
                case 1:
                    break;
            }
        }
    }

    public void PlayerHPSet()
    {
        int hp = 0;
        int def = 0;
        for (int i = 0; i < PartyIndex.Count; ++i)
        {
            if (PartyIndex[i].playerRare == true)
            {
                hp += PlayerSRare[PartyIndex[i].playerNum].maxHP + PlayerSRare[PartyIndex[i].playerNum].levelUpHP * (PartyIndex[i].playerLevel - 1) + PlayerSRare[PartyIndex[i].playerNum].starUpHP * (PartyIndex[i].playerStar - 1);
                def += PlayerSRare[PartyIndex[i].playerNum].def + PlayerSRare[PartyIndex[i].playerNum].levelUpDef * (PartyIndex[i].playerLevel - 1) + PlayerSRare[PartyIndex[i].playerNum].starUpDef * (PartyIndex[i].playerStar - 1);
            }
            else
            {
                hp += PlayerRare[PartyIndex[i].playerNum].maxHP + PlayerRare[PartyIndex[i].playerNum].levelUpHP * (PartyIndex[i].playerLevel - 1) + PlayerRare[PartyIndex[i].playerNum].starUpHP * (PartyIndex[i].playerStar - 1);
                def += PlayerRare[PartyIndex[i].playerNum].def + PlayerRare[PartyIndex[i].playerNum].levelUpDef * (PartyIndex[i].playerLevel - 1) + PlayerRare[PartyIndex[i].playerNum].starUpDef * (PartyIndex[i].playerStar - 1);
            }
        }

        partyHP = hp;
        partyDef = def;

    }

    public int GetPartyAtk(int partyNum, bool rare)
    {
        int num = PartyIndex[partyNum].playerNum;

        if(rare)
        {
            return (PlayerSRare[num].damage + PlayerSRare[num].levelUpDamage * (PartyIndex[partyNum].playerLevel - 1) + PlayerSRare[num].starUpDamage * (PartyIndex[partyNum].playerStar - 1));
        }
        else
        {
            return (PlayerRare[num].damage + PlayerRare[num].levelUpDamage * (PartyIndex[partyNum].playerLevel - 1) + PlayerRare[num].starUpDamage * (PartyIndex[partyNum].playerStar - 1));
        }
    }

    public int GetPartyAllAtk()
    {
        int AllAtk = 0;
        for (int i = 0; i < PartyIndex.Count; ++i)
        {
            int num = PartyIndex[i].playerNum;
            bool rare = PartyIndex[i].playerRare;

            if (rare)
            {
                AllAtk += PlayerSRare[num].damage + PlayerSRare[num].levelUpDamage * (PartyIndex[i].playerLevel - 1) + PlayerSRare[num].starUpDamage * (PartyIndex[i].playerStar - 1);
            }
            else
            {
                AllAtk += PlayerRare[num].damage + PlayerRare[num].levelUpDamage * (PartyIndex[i].playerLevel - 1) + PlayerRare[num].starUpDamage * (PartyIndex[i].playerStar - 1);
            }
        }
        return AllAtk;
    }

    public float GetPartyCri(int partyNum, bool rare)
    {
        int num = PartyIndex[partyNum].playerNum;

        if (rare)
        {
            return (PlayerSRare[num].cri + PlayerSRare[num].levelUpCri * (PartyIndex[partyNum].playerLevel - 1) + PlayerSRare[num].starUpCri * (PartyIndex[partyNum].playerStar - 1));
        }
        else
        {
            return (PlayerRare[num].cri + PlayerRare[num].levelUpCri * (PartyIndex[partyNum].playerLevel - 1) + PlayerRare[num].starUpCri * (PartyIndex[partyNum].playerStar - 1));
        }
    }

    public void GetGold(int count)
    {
        gold += count;
    }

    public void GetCash(int count)
    {
        cash += count;
    }
}
