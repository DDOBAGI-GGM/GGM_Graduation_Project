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


// 이거 코드 안쓸 듯
// 관련된거 주석처리했음. 나중에 지워주기!