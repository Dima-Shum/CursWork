using System.Collections;
using UnityEngine;

public class TrancpenceryDetection : MonoBehaviour
{
    private const float FULL_NON_TRANCPARENT = 1.0f;

    [Range(0f, 1f)]

    [SerializeField] private float TrancperencyAmount = 0.8f;
    [SerializeField] private float FadeTime = 0.5f;

    private SpriteRenderer _spriteRenderer;

    


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Player>())
        {
            if(collider is CapsuleCollider2D)
                StartCoroutine(FadeRoutine(_spriteRenderer,FadeTime, _spriteRenderer.color.a, TrancperencyAmount));
        }    
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            if(collider is CapsuleCollider2D)
                StartCoroutine(FadeRoutine(_spriteRenderer, FadeTime, _spriteRenderer.color.a, FULL_NON_TRANCPARENT));
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float FadeTime, float startTrancperencyAmount, float targetTrancperencyAmount)
    {
        float elapseTime = 0f;

        while(elapseTime < FadeTime)
        {
            elapseTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTrancperencyAmount, targetTrancperencyAmount, elapseTime / FadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

            yield return null;
        }
    }
}
