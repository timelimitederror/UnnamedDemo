using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class InitManager : MonoBehaviourSinqletonBase<InitManager>
{
    public const string SKILL_1 = "skill_1";
    public const string SKILL_2 = "skill_2";
    public const string SKILL_3 = "skill_3";
    public const string SKILL_4 = "skill_4";
    public const string MIXING_COLOR = "mixingColor";
    public const string SWITCH_COLOR = "switchColor";

    private const string DEFAULT_INIT =
        @"
{
    ""keyPosition"":{
        ""skill_1"":49,
        ""skill_2"":50,
        ""skill_3"":51,
        ""skill_4"":52,
        ""switchColor"":114,
        ""mixingColor"":102
    },
    ""voice"":{
        ""bgmVolume"":0.2,
        ""soundVolume"":0.5,
        ""bgmIsMute"":false,
        ""soundIsMute"":false
    }
}";

    private string initPath;

    private Dictionary<string, JToken> initDictionary;
    private JObject keyPositionInit;

    public override void Awake()
    {
        base.Awake();
        initPath = Path.Combine(Application.persistentDataPath, "init.json");
        string initJsonString;
        if (!File.Exists(initPath))
        {
            File.WriteAllText(initPath, DEFAULT_INIT, Encoding.UTF8);
            initJsonString = DEFAULT_INIT;
        }
        else
        {
            initJsonString = File.ReadAllText(initPath, Encoding.UTF8);
        }

        JObject init = JObject.Parse(initJsonString);
        initDictionary = new Dictionary<string, JToken>();
        foreach (JProperty property in init.Properties())
        {
            initDictionary[property.Name] = property.Value;
        }
        keyPositionInit = (JObject)initDictionary["keyPosition"];
    }

    public JToken GetInitByName(string name)
    {
        if (initDictionary.ContainsKey(name))
        {
            return initDictionary[name];
        }
        return null;
    }

    public void ReplaceInit(string name, JToken newInit)
    {
        initDictionary[name] = newInit;
    }

    public void SaveInit()
    {
        File.WriteAllText(initPath, JsonConvert.SerializeObject(initDictionary), Encoding.UTF8);
    }

    public PlayerSkillKeyPositionChanged GetKeyPosition()
    {
        return new PlayerSkillKeyPositionChanged(
            (KeyCode)(int)keyPositionInit[SKILL_1],
            (KeyCode)(int)keyPositionInit[SKILL_2],
            (KeyCode)(int)keyPositionInit[SKILL_3],
            (KeyCode)(int)keyPositionInit[SKILL_4],
            (KeyCode)(int)keyPositionInit[MIXING_COLOR],
            (KeyCode)(int)keyPositionInit[SWITCH_COLOR]
            );
    }

    public void ReplaceKeyPosition(string name, KeyCode newKey)
    {
        keyPositionInit[name] = (int)newKey;

        EventBus.Publish(new PlayerSkillKeyPositionChanged(
            (KeyCode)(int)keyPositionInit[SKILL_1],
            (KeyCode)(int)keyPositionInit[SKILL_2],
            (KeyCode)(int)keyPositionInit[SKILL_3],
            (KeyCode)(int)keyPositionInit[SKILL_4],
            (KeyCode)(int)keyPositionInit[MIXING_COLOR],
            (KeyCode)(int)keyPositionInit[SWITCH_COLOR]
            ));
    }
}
