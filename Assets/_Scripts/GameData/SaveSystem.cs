using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

public static class SaveSystem 
{

    public static void SavePlayerData(DataManager data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bert";
        FileStream stream = new FileStream(path, FileMode.Create);
        BinaryDataSaver dataFile = new BinaryDataSaver(data);

        formatter.Serialize(stream, dataFile);
        stream.Close();
        
    }

    public static BinaryDataSaver LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.bert";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        BinaryDataSaver data = formatter.Deserialize(stream) as BinaryDataSaver;
        stream.Close();
            
        return data;
        

    }
    
}
