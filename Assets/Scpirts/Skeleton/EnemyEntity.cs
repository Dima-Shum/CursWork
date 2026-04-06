using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent (typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{

    [SerializeField] private EnemySO _enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
  
    private GameObject _victoryPanel;

    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>(); 
        _enemyAI = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;

        _victoryPanel = GameObject.Find("victoryPanel");
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(false);
            Debug.Log("Панель победы скрыта при старте");
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);

            // Добавляем убийство для ЛЮБОГО врага (включая босса?)
            KillCounter.instance.AddKill();

            if (gameObject.CompareTag("Boss"))
            {
                // Получаем время и убийства
                string victoryTime = GameTimer.instance.GetFormattedTime(); // нужно добавить этот метод в GameTimer
                string victoryKills = KillCounter.instance.GetKillCount().ToString();

                // Показываем панель и заполняем текст
                ShowVictory(victoryTime, victoryKills);
            }
        }
    }

    private void ShowVictory(string time, string kills)
    {
        // Сохраняем данные
        GlobalGameData.GameResult = "Victory";
        GlobalGameData.FinalTime = time;
        GlobalGameData.FinalKills = kills;
        StatsManager.Instance.SaveCurrentSession("Victory", time, kills);

        // Выводим в консоль
        Debug.Log($"=== РЕЗУЛЬТАТ ИГРЫ ===\nИгрок: {GlobalGameData.PlayerName}\nСтатус: {GlobalGameData.GameResult}\nВремя: {GlobalGameData.FinalTime}\nУбийств: {GlobalGameData.FinalKills}");
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform winContainer = canvas.transform.Find("WinContainer");
            if (winContainer != null)
            {
                Transform victoryPanelTransform = winContainer.Find("victoryPanel");
                if (victoryPanelTransform != null)
                {
                    TMP_Text timeText = victoryPanelTransform.Find("victoryTime")?.GetComponent<TMP_Text>();
                    if (timeText != null) timeText.text = $"Time: {time}";

                    TMP_Text killsText = victoryPanelTransform.Find("victoryKills")?.GetComponent<TMP_Text>();
                    if (killsText != null) killsText.text = $"Kills: {kills}";

                    victoryPanelTransform.gameObject.SetActive(true);
                    StartCoroutine(ReturnToMenu());
                    return;
                }
            }
        }

        // Fallback
        GameObject victoryPanel = GameObject.Find("victoryPanel");
        if (victoryPanel != null)
        {
            TMP_Text timeText = victoryPanel.transform.Find("victoryTime")?.GetComponent<TMP_Text>();
            if (timeText != null) timeText.text = $"Time: {time}";

            TMP_Text killsText = victoryPanel.transform.Find("victoryKills")?.GetComponent<TMP_Text>();
            if (killsText != null) killsText.text = $"Kills: {kills}";

            victoryPanel.SetActive(true);
            StartCoroutine(ReturnToMenu());
        }
    }
    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }


}
