using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<FighterAgent> agents;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(int episodesCompletedSoFar)
    {
        foreach (FighterAgent agent in agents)
        {
            agent.EndEpisodeAndLogData();
        }
        // Inform the DataManager that a new episode is beginning.
        DataManager.Instance.OnNewEpisode(episodesCompletedSoFar + 1);
    }
}
