using UnityEngine;
using CustomUtility.IO;

public class PlayerSaveDataExample : SaveData
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Damage { get; set; }
    

    public PlayerSaveDataExample()
    {
    }

    public PlayerSaveDataExample(string name, int hp, int damage)
    {
        Name = name;
        Hp = hp;
        Damage = damage;
    }

}

public class SaveTestSample : MonoBehaviour
{
    private PlayerSaveDataExample _jsonSave;
    private PlayerSaveDataExample _jsonLoad;
    private PlayerSaveDataExample _binarySave;
    private PlayerSaveDataExample _binaryLoad;

    private void Start()
    {
        //LoadJson();
        SaveJson();
        LoadJson();
        
        //SaveBinary();
        //LoadBinary();
    }

    private void SaveJson()
    {
        _jsonSave = new("뇸뇸이", 500, 20);
        
        DataSaveController.Save(_jsonSave, SaveType.JSON);
    }

    private void LoadJson()
    {
        _jsonLoad = new("", 0, 0);

        DataSaveController.Load(ref _jsonLoad, SaveType.JSON);
        Debug.Log(_jsonLoad.Name);
        Debug.Log(_jsonLoad.Hp);
        Debug.Log(_jsonLoad.Damage);
    }

    private void SaveBinary()
    {
        _jsonSave = new("용용이", 100, 20);
        _jsonSave.Hp = 76;

        DataSaveController.Save(_jsonSave, SaveType.BINARY);
    }

    private void LoadBinary()
    {
        _jsonLoad = new("", 0, 0);

        DataSaveController.Load(ref _jsonLoad, SaveType.BINARY);
        Debug.Log(_jsonLoad.Hp);
    }
}
