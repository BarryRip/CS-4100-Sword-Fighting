using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

/// <summary>
/// Singleton script that allows other scripts to log data into a file.
/// <para/>
/// To use this, make sure the script exists in the scene. Agent scripts can call RegisterAgent()
/// to receive an AgentData object. Modify fields in that AgentData object to store information.
/// <para/>
/// The data manager automatically divides data by episode, and once the application ends,
/// saves all data in the Assets/Data folder as both a .txt and .csv file.
/// </summary>
public class DataManager : MonoBehaviour
{
    // Part of the singleton pattern
    public static DataManager Instance { get; private set; }
    // Stores a list of all registered data objects
    private List<AgentData> activeAgentDataList;
    // A construct similar to a 3D array that stores all agent data indexed by episode, agent id, and field name.
    private AgentData3DArray storedAgentDataArray;

    private void Awake()
    {
        // Part of singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        activeAgentDataList = new List<AgentData>();
        storedAgentDataArray = new AgentData3DArray();
    }

    private void OnApplicationQuit()
    {
        // When app quits, save data in .txt and .csv files.
        string dirPath = Application.dataPath + "/Data";
        Directory.CreateDirectory(dirPath);
        string fileTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        File.WriteAllText(dirPath + "/" + fileTime + ".txt", ToString());
        string csvText = storedAgentDataArray.ToCsvFormat();
        if (!csvText.Equals(""))
        {
            File.WriteAllText(dirPath + "/" + fileTime + ".csv", csvText);
        }
        else
        {
            Debug.LogWarning("Simulation ended before data was collected. Csv file could not be generated.");
        }
    }

    /// <summary>
    /// Registers an agent data object to keep track of data for the given object. Can
    /// optionally be given a name for display.
    /// </summary>
    /// <returns>An AgentData object to log data into.</returns>
    public AgentData RegisterAgent(string name = "")
    {
        AgentData newData = new AgentData(name);
        activeAgentDataList.Add(newData);
        return newData;
    }

    public override string ToString()
    {
        return storedAgentDataArray.ToString();
    }

    /// <summary>
    /// Stores the data from all active AgentData objects into the 3D array construct.
    /// Also resets each AgentData before the start of the next episode.
    /// <para/>
    /// This method must manually be called by a script that tracks the end / start of an episode,
    /// such as the GameController.
    /// </summary>
    /// <param name="episode">The number of the episode that has elapsed and is about to end.</param>
    public void OnNewEpisode(int episode)
    {
        foreach (AgentData data in activeAgentDataList)
        {
            data.PushIntoDataArray(storedAgentDataArray, episode);
        }
    }
}

/// <summary>
/// Stores data for agents.
/// </summary>
public class AgentData
{
    // TODO: Add more fields here to add data to track. If you update this,
    // don't forget to update the ResetAgentData method and modify the field
    // in the appropriate location in the FighterAgent script.
    public int hitsTaken;
    public int hitsDealt;
    public float damageTaken;
    public float damageDealt;
    public float healthAtEnd;
    public float rewardAtEnd;

    private int id;
    private string name;
    private static int nextId = 1;

    public AgentData(string name)
    {
        this.name = name;
        this.id = nextId++;
    }

    /// <summary>
    /// Logs that a certain amount of damage was dealt and updates all related fields.
    /// </summary>
    /// <param name="damage">The amount of damage dealt.</param>
    public void LogHitDealt(float damage)
    {
        hitsDealt += 1;
        damageDealt += damage;
    }

    /// <summary>
    /// Logs that a certain amount of damage was taken and updates all related fields.
    /// </summary>
    /// <param name="damage">The amount of damage received.</param>
    public void LogHitTaken(float damage)
    {
        hitsTaken += 1;
        damageTaken += damage;
    }

    /// <summary>
    /// Pushes the data in this AgentData into the given 3D array construct,
    /// and then resets the stored data fields.
    /// </summary>
    /// <param name="dataArray">The array to push the data into.</param>
    /// <param name="episode">The episode that this data represents.</param>
    public void PushIntoDataArray(AgentData3DArray dataArray, int episode)
    {
        dataArray.PushAgentData(episode, this);
        ResetAgentData();
    }

    /// <summary>
    /// Gets the id of the agent's data. This is mostly used as a unique
    /// identifier when storing the data in a dictionary.
    /// </summary>
    /// <returns>The identifier for this data as a string.</returns>
    public string GetStringId()
    {
        if (!name.Equals(""))
        {
            return name + " (id=" + id + ")";
        }
        return "AgentId" + id; 
    }

    private void ResetAgentData()
    {
        hitsTaken = 0;
        hitsDealt = 0;
        damageTaken = 0;
        damageDealt = 0;
        healthAtEnd = 0;
        rewardAtEnd = 0;
    }
}

/// <summary>
/// Simulates a 3D array-like structure, organizing data by episode, agent data identifier, and field name.
/// <para/>
/// This structure is used by DataManager to store the data for all agents at each episode.
/// </summary>
public class AgentData3DArray
{
    private List<Dictionary<string, Dictionary<string, object>>> dictionaryList;

    public AgentData3DArray()
    {
        dictionaryList = new List<Dictionary<string, Dictionary<string, object>>>();
    }

    /// <summary>
    /// Gets the dictionary containing agent name keys to agent field dictionary values.
    /// </summary>
    /// <param name="episode">The episode this data occurred in.</param>
    /// <returns>A dictionary of agent name keys to agent field values.</returns>
    public Dictionary<string, Dictionary<string, object>> GetDictionaryAtEpisode(int episode)
    {
        while (dictionaryList.Count < episode)
        {
            dictionaryList.Add(new Dictionary<string, Dictionary<string, object>>());
        }
        return dictionaryList[episode - 1];
    }

    /// <summary>
    /// Gets the dictionary containing agent field name keys to field values.
    /// </summary>
    /// <param name="episode">The episode this data occurred in.</param>
    /// <param name="agentName">The identifier of the agent data.</param>
    /// <returns>A dictionary of field name keys to field values.</returns>
    public Dictionary<string, object> GetAgentDict(int episode, string agentName)
    {
        var dict = GetDictionaryAtEpisode(episode);
        if (!dict.ContainsKey(agentName))
        {
            dict.Add(agentName, new Dictionary<string, object>());
        }
        return dict[agentName];
    }

    /// <summary>
    /// Gets the value of a field for an agent at the end of an episode.
    /// </summary>
    /// <param name="episode">The episode this data occurred in.</param>
    /// <param name="agentName">The identifier of the agent data.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The value of the given field, returned as an object type.</returns>
    public object GetField(int episode, string agentName, string fieldName)
    {
        var dict = GetAgentDict(episode, agentName);
        if (!dict.ContainsKey(fieldName))
        {
            dict.Add(fieldName, null);
        }
        return dict[fieldName];
    }

    /// <summary>
    /// Pushes the data from the given AgentData into the 3D array construct.
    /// </summary>
    /// <param name="episode">The episode this data occurred in.</param>
    /// <param name="agentData">The data to push into the array.</param>
    public void PushAgentData(int episode, AgentData agentData)
    {
        Dictionary<string, object> agentDict = GetAgentDict(episode, agentData.GetStringId());
        foreach (var field in agentData.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            var value = field.GetValue(agentData);
            agentDict[field.Name] = value;
        }
    }

    public override string ToString()
    {
        string str = "DATA (finished running at " + System.DateTime.Now + ")\n\n";
        for (int i = 0; i < dictionaryList.Count; i++)
        {
            var dict = dictionaryList[i];
            str += "Episode " + (i + 1) + ":\n";
            foreach (var agentDictPair in dict)
            {
                str += "\t" + agentDictPair.Key + ":\n";
                foreach (var fieldObjectPair in agentDictPair.Value)
                {
                    str += "\t\t" + fieldObjectPair.Key + " = " + fieldObjectPair.Value + "\n";
                }
            }
        }
        return str;
    }

    /// <summary>
    /// Converts the stored data into CSV format.
    /// </summary>
    /// <returns>The data represented as a string in CSV format.</returns>
    public string ToCsvFormat()
    {
        if (dictionaryList.Count == 0)
        {
            return "";
        }
        // Create header
        string csv = "Episode";
        List<string> agentRowNames = new List<string>(dictionaryList.Count);
        for (int i = 0; i < dictionaryList.Count; i++)
        {
            csv += "," + (i+1);
        }
        csv += "\n\n";
        // Create rows for each data field
        foreach (string agentName in dictionaryList[0].Keys)
        {
            foreach (string fieldName in dictionaryList[0][agentName].Keys)
            {
                csv += agentName + "." + fieldName;
                for (int i = 0; i < dictionaryList.Count; i++)
                {
                    if (!dictionaryList[i].ContainsKey(agentName)) continue;
                    csv += "," + dictionaryList[i][agentName][fieldName];
                }
                csv += "\n";
            }
            csv += "\n";
        }
        return csv;
    }
}
