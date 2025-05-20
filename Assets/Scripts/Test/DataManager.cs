using System.Collections;
using System.Collections.Generic;
using CustomUtility.IO;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [field: SerializeField] public CsvTable MonsterCSV {  get; private set; }
    [field: SerializeField] public CsvDictionary MonsterDic { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        CsvReader.Read(MonsterCSV);
        CsvReader.Read(MonsterDic);
    }
}

public enum MonsterData
{
    Name = 1, Atk, Dfe, Spd, Dsc
}
