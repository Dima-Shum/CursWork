using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
   public void LoadScenes(int index)
    {
        SceneManager.LoadScene(index);
    }
}
