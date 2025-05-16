using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillKeyPositionChanged
{
    public static Dictionary<KeyCode, string> mappingTable = new Dictionary<KeyCode, string>
    {
        {KeyCode.Alpha0, "0"},
        {KeyCode.Alpha1, "1"},
        {KeyCode.Alpha2, "2"},
        {KeyCode.Alpha3, "3"},
        {KeyCode.Alpha4, "4"},
        {KeyCode.Alpha5, "5"},
        {KeyCode.Alpha6, "6"},
        {KeyCode.Alpha7, "7"},
        {KeyCode.Alpha8, "8"},
        {KeyCode.Alpha9, "9"},
        {KeyCode.A, "A"},
        {KeyCode.B, "B"},
        {KeyCode.C, "C"},
        {KeyCode.D, "D"},
        {KeyCode.E, "E"},
        {KeyCode.F, "F"},
        {KeyCode.G, "G"},
        {KeyCode.H, "H"},
        {KeyCode.I, "I"},
        {KeyCode.J, "J"},
        {KeyCode.K, "K"},
        {KeyCode.L, "L"},
        {KeyCode.M, "M"},
        {KeyCode.N, "N"},
        {KeyCode.O, "O"},
        {KeyCode.P, "P"},
        {KeyCode.Q, "Q"},
        {KeyCode.R, "R"},
        {KeyCode.S, "S"},
        {KeyCode.T, "T"},
        {KeyCode.U, "U"},
        {KeyCode.V, "V"},
        {KeyCode.W, "W"},
        {KeyCode.X, "X"},
        {KeyCode.Y, "Y"},
        {KeyCode.Z, "Z"},
        {KeyCode.BackQuote, "`"},
        {KeyCode.Minus, "-"},
        {KeyCode.Equals, "="},
        {KeyCode.LeftBracket, "["},
        {KeyCode.RightBracket, "]"},
        {KeyCode.Backslash, "\\"},
        {KeyCode.Semicolon, ";"},
        {KeyCode.Quote, "'"},
        {KeyCode.Comma, ","},
        {KeyCode.Period, "."},
        {KeyCode.Slash, "/"},
    };

    public string skill_1;
    public string skill_2;
    public string skill_3;
    public string skill_4;
    public string mixing;
    public string armColor;

    public KeyCode skill_1Key;
    public KeyCode skill_2Key;
    public KeyCode skill_3Key;
    public KeyCode skill_4Key;
    public KeyCode mixingKey;
    public KeyCode armColorKey;

    public PlayerSkillKeyPositionChanged(
        KeyCode skill_1Key, KeyCode skill_2Key, KeyCode skill_3Key, KeyCode skill_4Key,
        KeyCode mixingKey, KeyCode armColorKey)
    {
        this.skill_1 = mappingTable.ContainsKey(skill_1Key) ? mappingTable[skill_1Key] : skill_1Key.ToString();
        this.skill_1Key = skill_1Key;
        this.skill_2 = mappingTable.ContainsKey(skill_2Key) ? mappingTable[skill_2Key] : skill_2Key.ToString();
        this.skill_2Key = skill_2Key;
        this.skill_3 = mappingTable.ContainsKey(skill_3Key) ? mappingTable[skill_3Key] : skill_3Key.ToString();
        this.skill_3Key = skill_3Key;
        this.skill_4 = mappingTable.ContainsKey(skill_4Key) ? mappingTable[skill_4Key] : skill_4Key.ToString();
        this.skill_4Key = skill_4Key;
        this.mixing = mappingTable.ContainsKey(mixingKey) ? mappingTable[mixingKey] : mixingKey.ToString();
        this.mixingKey = mixingKey;
        this.armColor = mappingTable.ContainsKey(armColorKey) ? mappingTable[armColorKey] : armColorKey.ToString();
        this.armColorKey = armColorKey;
    }
}
