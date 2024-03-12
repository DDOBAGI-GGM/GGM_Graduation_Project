using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.ComponentModel;
using UnityEditor.Rendering;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float currentTime;
    bool isDie = false;

    void SetTime()
    {
        isDie = false;
        currentTime = GameManager.Instance.nowStageData.gameTime;
    }

    private void Start()
    {
        SetTime();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timer.text = Mathf.Round(currentTime).ToString() + " s";
        }
        else if (isDie == false)
        {
            isDie = !isDie;
            timer.text = "0 s";
            TimeOutManager.Instance.timeOutShow();
        }        
    }
}
