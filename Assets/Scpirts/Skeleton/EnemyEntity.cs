using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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
            Debug.Log("ѕанель победы скрыта при старте");
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

            if(gameObject.CompareTag("Boss"))
            {
                ShowVictory();
            }

        }
    }

    private void ShowVictory()
    {
        // »щем через Canvas (как в диагностике)
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform winContainer = canvas.transform.Find("WinContainer");
            if (winContainer != null)
            {
                Transform victoryPanelTransform = winContainer.Find("victoryPanel");
                if (victoryPanelTransform != null)
                {
                    victoryPanelTransform.gameObject.SetActive(true);
                    StartCoroutine(ReturnToMenu());
                    return;
                }
            }
        }

        // ≈сли не нашли через путь - пробуем старый способ
        GameObject victoryPanel = GameObject.Find("victoryPanel");
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            StartCoroutine(ReturnToMenu());
        }
        else
        {
            Debug.LogError("victoryPanel не найден!");
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }


}
