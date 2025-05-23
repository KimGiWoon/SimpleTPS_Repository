using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public DataManager Data;

    [SerializeField] string _name;
    [SerializeField] int _atk;
    [SerializeField] int _dfe;
    [SerializeField] int _spd;
    [SerializeField] string _dsc;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Init(1);
        }
    }

    private void Init(int number)
    {
        Debug.Log(Data.MonsterCSV.GetData(number, (int)MonsterData.Name));

        _name = Data.MonsterCSV.GetData(number, (int)MonsterData.Name);
        _atk = int.Parse(Data.MonsterCSV.GetData(number, (int)MonsterData.Atk));
        _dfe = int.Parse(Data.MonsterCSV.GetData(number, (int)MonsterData.Dfe));
        _spd = int.Parse(Data.MonsterCSV.GetData(number, (int)MonsterData.Spd));
        _dsc = Data.MonsterCSV.GetData(number, (int)MonsterData.Dsc);
    }
}
