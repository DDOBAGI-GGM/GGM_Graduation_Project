using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    [SerializeField] private Toggle fullscreenBtn;

    private List<Resolution> resolutionList = new List<Resolution>();
    private TMP_Dropdown resolutionDropdown;
    [SerializeField] private int resolutionIndex;

    private void Start()
    {
        resolutionDropdown = GetComponent<TMP_Dropdown>();
        InitUI();
    }

    private void InitUI()
    {
        resolutionList.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();     // 안에 있는 리스트 초기화

        resolutionIndex = 0;

        foreach (Resolution r in resolutionList)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{r.width} X {r.height}";
            resolutionDropdown.options.Add(option);

            if (r.width == Screen.width && r.height == Screen.height)
            {
                resolutionDropdown.value = resolutionIndex;
            }
            resolutionIndex++;
        }

        resolutionDropdown.RefreshShownValue();     // 새로고침

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;   
    }

    public void DropboxOptionChange(int index)
    {
        resolutionIndex = index;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutionList[resolutionIndex].width, resolutionList[resolutionIndex].height, screenMode);    
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;        // 참이면 풀스크린.
    }
}
