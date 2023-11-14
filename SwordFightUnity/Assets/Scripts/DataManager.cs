using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Singleton script that allows other scripts to log data into a file.
/// <para/>
/// To use this, make sure the script exists in the scene. Any scripts, mainly
/// agents and the environment, can call Register() to receive a Data object.
/// Call Log() on that Data object to store information in that object.
/// </summary>
public class DataManager : MonoBehaviour
{
    // Part of the singleton pattern
    public static DataManager Instance { get; private set; }
    // Stores a list of all registered data objects
    private List<Data> dataList;

    private void Start()
    {
        // Part of singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Awake()
    {
        dataList = new List<Data>();
    }

    private void OnApplicationQuit()
    {
        // On quit, save a file with the data collected.
        string path = Application.dataPath + "/DATA.txt";
        int copyNumber = 1;
        while (File.Exists(path))
        {
            copyNumber++;
            path = Application.dataPath + "/DATA_" + copyNumber + ".txt";
        }
        File.WriteAllText(path, Stringify());
    }

    /// <summary>
    /// Registers a data object to keep track of data for the given object.
    /// </summary>
    /// <returns>A Data object to log data into.</returns>
    public Data Register(string name)
    {
        Data newData = new Data(name);
        dataList.Add(newData);
        return newData;
    }

    /// <summary>
    /// Returns a string representing all data collected thus far.
    /// </summary>
    /// <returns>A string with the formatted data.</returns>
    public string Stringify()
    {
        string fullStr = "DATA (run ended on " + System.DateTime.Now + ")\n\n";
        foreach (Data data in dataList)
        {
            fullStr += data.StringifyData() + "\n";
        }
        return fullStr;
    }
}

/// <summary>
/// Stores a dictionary of data that can be written into.
/// </summary>
public class Data
{
    private static int nextId = 0;
    private int id;
    private string name;
    private Dictionary<string, object> dataDict;

    public Data(string name)
    {
        this.id = nextId++;
        this.name = name;
        this.dataDict = new Dictionary<string, object>();
    }

    /// <summary>
    /// Log data into the object. If given an existing key string, overwrites the previous data.
    /// </summary>
    /// <param name="str">The string key to save the data under.</param>
    /// <param name="obj">The value of the data to save.</param>
    public void Log(string str, object obj)
    {
        bool keyExists = dataDict.ContainsKey(str);
        if (!keyExists)
        {
            dataDict.Add(str, obj);
            return;
        }
        dataDict[str] = obj;
    }

    /// <summary>
    /// Creates a string representing the data contained in the object.
    /// </summary>
    /// <returns>A formatted string representing the data.</returns>
    public string StringifyData()
    {
        string dataStr = name + " (id = " + id + "):\n";
        foreach (KeyValuePair<string, object> entry in dataDict)
        {
            dataStr += "\t" + entry.Key + ": " + entry.Value + "\n";
        }
        return dataStr;
    }
}
