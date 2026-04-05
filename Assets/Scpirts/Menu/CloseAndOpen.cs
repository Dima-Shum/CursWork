using UnityEngine;
using UnityEngine.UI;

public class CloseAndOpen : MonoBehaviour
{
    [Header("Панели")]
    public GameObject controlPanel;
    public GameObject statsPanel;

    [Header("Кнопки")]
    public Button controlButton;
    public Button statisticsButton;

    private CanvasGroup controlCG;
    private CanvasGroup statsCG;

    void Start()
    {
        // Получаем CanvasGroup или добавляем, если нет
        controlCG = GetOrAddCanvasGroup(controlPanel);
        statsCG = GetOrAddCanvasGroup(statsPanel);

        // Скрываем обе панели при старте
        SetPanelVisibility(controlCG, false);
        SetPanelVisibility(statsCG, false);

        // Назначаем обработчики кнопок
        if (controlButton != null)
            controlButton.onClick.AddListener(ToggleControlPanel);

        if (statisticsButton != null)
            statisticsButton.onClick.AddListener(ToggleStatsPanel);
    }

    void ToggleControlPanel()
    {
        bool isVisible = controlCG.alpha == 1;
        SetPanelVisibility(controlCG, !isVisible);

        // Опционально: при открытии ControlPanel закрываем StatsPanel
        if (!isVisible && statsCG.alpha == 1)
        {
            SetPanelVisibility(statsCG, false);
        }
    }

    void ToggleStatsPanel()
    {
        bool isVisible = statsCG.alpha == 1;
        SetPanelVisibility(statsCG, !isVisible);

        // Опционально: при открытии StatsPanel закрываем ControlPanel
        if (!isVisible && controlCG.alpha == 1)
        {
            SetPanelVisibility(controlCG, false);
        }
    }

    void SetPanelVisibility(CanvasGroup cg, bool visible)
    {
        cg.alpha = visible ? 1f : 0f;
        cg.interactable = visible;
        cg.blocksRaycasts = visible;
    }

    CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.AddComponent<CanvasGroup>();
        return cg;
    }
}