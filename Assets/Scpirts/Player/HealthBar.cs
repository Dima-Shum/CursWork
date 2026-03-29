using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    public Player player;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        if (player != null && healthBar != null)
        {
            healthBar.fillAmount = (float)player._currentHealth / player._maxHealth;
        }
    }
}



