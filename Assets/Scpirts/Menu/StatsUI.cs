using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statsText; // ѕеретащить TMP_Text в инспекторе

    private void Start()
    {
        DisplayStats();
    }

    private void OnEnable()
    {
        DisplayStats(); // ќбновл€ем при каждом показе (если панель скрываетс€/показываетс€)
    }

    public void DisplayStats()
    {
        List<GameSessionData> sessions = StatsManager.Instance.GetLastSessions();

        if (sessions.Count == 0)
        {
            statsText.text = "No load stats";
            return;
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < sessions.Count; i++)
        {
            GameSessionData session = sessions[i];
            sb.AppendLine($"{i + 1}. {session.playerName} - {session.result}");
            sb.AppendLine($"   Time: {session.time} | Kills: {session.kills}");
            sb.AppendLine();
        }

        statsText.text = sb.ToString();
    }
}