using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    private int killCount = 0;
    public static KillCounter instance;

    // Публичные переменные для хранения результатов
    public string victoryKills = "";
    public string loseKills = "";

    private void Awake()
    {
        instance = this;
    }

    public void AddKill()
    {
        killCount++;
        Debug.Log($"Убийств: {killCount}");
    }

    public void ShowVictoryKills()
    {
        victoryKills = killCount.ToString();
        Debug.Log($"Победа! Убийств: {victoryKills}");
    }

    public void ShowLoseKills()
    {
        loseKills = killCount.ToString();
        Debug.Log($"Поражение! Убийств: {loseKills}");
    }

    public void ResetKills()
    {
        killCount = 0;
        victoryKills = "";
        loseKills = "";
    }

    public int GetKillCount()
    {
        return killCount;
    }
}