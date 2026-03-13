using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagabledObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.1f;

    private float blinkTimer;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;
    private bool isBlinking;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        isBlinking = true;
    }

    private void Start()
    {
        if (_damagabledObject is Player)
        {
            (_damagabledObject as Player).OnFlashBlink += DamagabledObject_OnFlashBlink;
        }
    }

    private void DamagabledObject_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkingMaterial();
    }

    private void Update()
    {
        if(isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if(blinkTimer < 0 )
            {
                SetDefaultMaterial();
            }
        }
    }

    private void SetBlinkingMaterial()
    {
        blinkTimer = _blinkDuration;
        spriteRenderer.material = _blinkMaterial;
    }

    private void SetDefaultMaterial()
    {
        spriteRenderer.material = defaultMaterial;
    }

    public void StopBlinking()
    {
        SetDefaultMaterial();
        isBlinking = false;
    }

    private void OnDestroy()
    {
        if (_damagabledObject is Player)
        {
            (_damagabledObject as Player).OnFlashBlink -= DamagabledObject_OnFlashBlink;
        }
    }
}
