using System;
using UnityEngine;

public class DesctructablePlant : MonoBehaviour
{


    public EventHandler OnDestructableTakeDamage;
    private void OnTriggerEnter2D(Collider2D collusion)
    {
        if(collusion.gameObject.GetComponent<Sword>())
        {
            OnDestructableTakeDamage.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);

            NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
        }
    }
}
