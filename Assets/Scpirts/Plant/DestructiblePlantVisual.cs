using System;
using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private DesctructablePlant desctructablePlant;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    private void Start()
    {
        desctructablePlant.OnDestructableTakeDamage += DesctructablePlant_OnDestructableTakeDamage;
    }

    private void DesctructablePlant_OnDestructableTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, desctructablePlant.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        desctructablePlant.OnDestructableTakeDamage -= DesctructablePlant_OnDestructableTakeDamage;
    }
}
