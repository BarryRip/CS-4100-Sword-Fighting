using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<FighterAgent> agents;
    private float startTime;
    private float timeLimit = 40f;
    private int episodesCompletedSoFar = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Time.time - startTime);
        if(Time.time - startTime > timeLimit)
        {
            foreach (FighterAgent agent in agents)
            {
                agent.TimeOut();
            }
            EndGame(episodesCompletedSoFar);
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
    }

    public void EndGame(int episodesCompletedSoFar)
    {
        foreach (FighterAgent agent in agents)
        {
            agent.EndEpisodeAndLogData();
        }
        episodesCompletedSoFar++;
        // Inform the DataManager that a new episode is beginning.
        DataManager.Instance.OnNewEpisode(episodesCompletedSoFar + 1);
        StartGame();
    }


}
