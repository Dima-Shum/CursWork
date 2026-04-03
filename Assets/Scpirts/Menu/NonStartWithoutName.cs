using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NonStartWithoutName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button StartGame;

    private void Start()
    {
        inputName.onValueChanged.AddListener(NameForStart);
        StartGame.interactable = false;
    }
    public void NameForStart(string newText)
    {
        StartGame.interactable = !string.IsNullOrEmpty(newText);
    }
}
