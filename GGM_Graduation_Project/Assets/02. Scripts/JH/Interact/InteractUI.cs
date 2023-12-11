using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
