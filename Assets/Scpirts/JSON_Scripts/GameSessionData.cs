[System.Serializable]
public class GameSessionData
{
    public string playerName;
    public string result;
    public string time;
    public string kills;

    public GameSessionData(string name, string result, string time, string kills)
    {
        this.playerName = name;
        this.result = result;
        this.time = time;
        this.kills = kills;
    }
}