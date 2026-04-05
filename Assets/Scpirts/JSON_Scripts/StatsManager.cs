using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private string savePath;
    private List<GameSessionData> sessions = new List<GameSessionData>();
    private const int MAX_SESSIONS = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "gameStats.json");
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCurrentSession(string result, string time, string kills)
    {
        string playerName = GlobalGameData.PlayerName;

        GameSessionData newSession = new GameSessionData(playerName, result, time, kills);
        sessions.Insert(0, newSession); // Новые в начало

        if (sessions.Count > MAX_SESSIONS)
        {
            sessions = sessions.Take(MAX_SESSIONS).ToList();
        }

        SaveToJson();
        Debug.Log($"Сохранена сессия: {playerName} - {result} - {time} - {kills} kills");
    }

    public List<GameSessionData> GetLastSessions()
    {
        return sessions;
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(new Wrapper { sessions = sessions }, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"JSON сохранён: {savePath}");
    }

    private void LoadStats()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            if (wrapper != null && wrapper.sessions != null)
            {
                sessions = wrapper.sessions;
                Debug.Log($"Загружено {sessions.Count} сессий");
            }
        }
        else
        {
            sessions = new List<GameSessionData>();
            Debug.Log("Новый файл статистики создан");
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<GameSessionData> sessions;
    }
}