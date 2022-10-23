using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
public abstract class GameDataManager<T>
{
    protected abstract string _fileName { get; }
    
    public void Save(T data)
    {
        string filepath = GetSavePath(_fileName);
        string serializedData = JsonConvert.SerializeObject(data);
        
        File.WriteAllText(filepath, serializedData);
    }
    
    public bool TryLoad(out T data)
    {
        data = default;
        string filepath = GetSavePath(_fileName);
        var exists = File.Exists(filepath);
        
        if (exists)
        {
            var serializedData = File.ReadAllText(filepath);
            data =  JsonConvert.DeserializeObject<T>(serializedData);
        }
        
        return exists;
    }

    private string GetSavePath(string dir)
    {
        char separator = Path.DirectorySeparatorChar;
        
        return $"{Application.persistentDataPath}{separator}{dir}";
    }
}
}