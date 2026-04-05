using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isGameRunning = true;

    public static GameTimer instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100) % 100);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void ShowVictory()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        string victoryTimeFormatted = FormatTime(elapsedTime);
        Debug.Log($"Победа! Время: {victoryTimeFormatted}");

        // Находим victoryPanel и обновляем текст victoryTime
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform winContainer = canvas.transform.Find("WinContainer");
            if (winContainer != null)
            {
                Transform victoryPanelTransform = winContainer.Find("victoryPanel");
                if (victoryPanelTransform != null)
                {
                    // Ищем объект victoryTime на панели
                    TMP_Text timeText = victoryPanelTransform.Find("victoryTime")?.GetComponent<TMP_Text>();
                    if (timeText != null)
                    {
                        timeText.text = victoryTimeFormatted;
                    }
                    else
                    {
                        Debug.LogWarning("Не найден объект victoryTime на victoryPanel");
                    }

                    victoryPanelTransform.gameObject.SetActive(true);
                    return;
                }
            }
        }

        // Fallback если не нашли через путь
        GameObject victoryPanel = GameObject.Find("victoryPanel");
        if (victoryPanel != null)
        {
            TMP_Text timeText = victoryPanel.transform.Find("victoryTime")?.GetComponent<TMP_Text>();
            if (timeText != null)
                timeText.text = victoryTimeFormatted;

            victoryPanel.SetActive(true);
        }
    }

    public void ShowLose()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        string loseTimeFormatted = FormatTime(elapsedTime);
        Debug.Log($"Поражение! Время: {loseTimeFormatted}");

        // Находим losePanel и обновляем текст loseTime
        GameObject losePanel = GameObject.Find("losePanel");
        if (losePanel != null)
        {
            TMP_Text timeText = losePanel.transform.Find("loseTime")?.GetComponent<TMP_Text>();
            if (timeText != null)
            {
                timeText.text = loseTimeFormatted;
            }
            else
            {
                Debug.LogWarning("Не найден объект loseTime на losePanel");
            }

            losePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("losePanel не найден!");
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isGameRunning = true;
    }

    public string GetFormattedTime()
    {
        return FormatTime(elapsedTime);
    }
}