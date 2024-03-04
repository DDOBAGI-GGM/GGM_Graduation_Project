using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HP : Singleton<HP>
{
    [SerializeField] private Slider Gage;
    [SerializeField] private int value;
    public Ease Ease;
    public void SetValue(bool player)
    {
        float gage = Gage.value;
        if (player)
            gage += value;
        else
            gage -= value;

        DOTween.To(() => Gage.value, x => Gage.value = x, gage, 1).SetEase(Ease);

        EndCheck();
    }

    private void EndCheck()
    {
        if (Gage.value <= 0 || Gage.value >= 100)
        {
            // ∞‘¿” ≥°...
        }
    }
}
