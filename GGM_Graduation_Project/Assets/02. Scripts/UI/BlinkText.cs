using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [Header("깜빡이는 속도")][SerializeField] private float _speed;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float alpha = (Mathf.Sin(Time.time * _speed) + 1f) / 2f;
        Color textColor = _text.color;
        textColor.a = alpha;
        _text.color = textColor;
    }
}

// 이거 코드 안쓸 듯