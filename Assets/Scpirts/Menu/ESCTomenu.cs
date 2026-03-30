using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCTomenu : MonoBehaviour
{

    [SerializeField] private float holdTime = 1.0f; 
    [SerializeField] private KeyCode targetKey = KeyCode.Escape; 

    private float holdTimer = 0f;
    private bool isHolding = false;

    void Update()
    {
        if (Input.GetKeyDown(targetKey))
        {
            isHolding = true;
            holdTimer = 0f;
            Debug.Log("Зажмите ESC на " + holdTime + " секунд для выхода");
        }

        if (Input.GetKey(targetKey) && isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                OnHoldComplete(0);
                isHolding = false; 
            }
        }

        if (Input.GetKeyUp(targetKey))
        {
            if (isHolding && holdTimer < holdTime)
            {
                Debug.Log("Отменено: нужно было удерживать дольше");
            }
            isHolding = false;
            holdTimer = 0f;
        }
    }

    private void OnHoldComplete(int index)
    {

        SceneManager.LoadScene(index);

        Debug.Log("ESC удержана! Выполняю действие...");
    }
}
