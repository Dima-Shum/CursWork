using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private const string IS_RUNNING = "IsRunning";

    private SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.instance.GetMousePosition();
        Vector3 PlayerPosition = Player.instance.GetPlayerScreenPosition();

        if(mousePos.x < PlayerPosition.x)
        {
            SpriteRenderer.flipX = true;
        }

        else
        {
            SpriteRenderer.flipX = false;
        }    

    }
}
