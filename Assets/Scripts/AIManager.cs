using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, SimpleMultiAgentGroup> _teams = new Dictionary<int, SimpleMultiAgentGroup>();

    [SerializeField]
    private AIController[] _agents;

    [SerializeField]
    private GameObject _ball;

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
        _ball.transform.position = new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
    }
}
