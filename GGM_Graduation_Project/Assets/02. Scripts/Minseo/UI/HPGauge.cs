using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField] protected float currentAiHp;
    [SerializeField] protected float currentPlayerHp;

    [SerializeField] private float maxHp;

    [SerializeField] private Slider _playerGauge;
    [SerializeField] private Slider _aiGauge;

    private void Start()
    {
        currentAiHp = maxHp;
        currentPlayerHp = currentAiHp;
    }

    private void UpdatePlayerSlider()
    {
        if (_playerGauge != null)
            _playerGauge.value = currentPlayerHp / maxHp;
    }

    private void UpdateAiSlider()
    {
        if (_aiGauge != null)
            _aiGauge.value = currentAiHp / maxHp;
    }

    public void PlayerDamage(float damage)
    {
        if (currentPlayerHp <= 0) 
            return;

        currentPlayerHp -= damage;
        UpdatePlayerSlider();

        if (currentPlayerHp <= 0)
        {
            Debug.Log("Player Die");
        }
    }

    public void AIDamage(float damage) 
    {
        if (currentAiHp <= 0)
            return;

        currentAiHp -= damage;
        UpdateAiSlider();

        if (currentAiHp <= 0)
        {
            Debug.Log("AI Die");
        }
    }

}