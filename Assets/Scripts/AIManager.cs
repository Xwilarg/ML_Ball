using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, SimpleMultiAgentGroup> _teams = new Dictionary<int, SimpleMultiAgentGroup>();

    [SerializeField]
    private AIController[] _agents;

    public GameObject Ball;

    private void Start()
    {
        foreach (var a in _agents)
        {
            if (!_teams.ContainsKey(a.TeamID))
            {
                var smag = new SimpleMultiAgentGroup();
                smag.RegisterAgent(a);
                _teams.Add(a.TeamID, smag);
            }
            else
                _teams[a.TeamID].RegisterAgent(a);
            a.Manager = this;
        }
        BeginGroupEpisode();
    }

    private void Update()
    {
        if (Ball.transform.localPosition.y < 0f || _agents.Any(x => x.transform.localPosition.y < 0f))
        {
            foreach (var t in _teams)
                t.Value.EndGroupEpisode();
            BeginGroupEpisode();
        }
    }

    public void TouchBall(int teamId)
    {
        foreach (var t in _teams)
        {
            t.Value.AddGroupReward(t.Key == teamId ? 1f : -1f);
            t.Value.EndGroupEpisode();
        }
        BeginGroupEpisode();
    }

    private void BeginGroupEpisode()
    {
        foreach (var a in _agents)
        {
            a.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        }
        Ball.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
    }
}
