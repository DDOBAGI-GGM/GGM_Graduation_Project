using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private CanvasGroup _canvasGroup;           // 알파값 조절
    [SerializeField] private float speed = 0;
    [SerializeField] private Ease outEase = Ease.OutCubic, inEase = Ease.InCubic;
    private Vector3 originPos;      // 0, 0, 0 화면이 다 구름에 가려져 있을 때임
    private Vector2 movePos;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        originPos = transform.localPosition;
    }

    public void Init()
    {
            originPos = new Vector2(originPos.x + 1080, originPos.y + 540);
    }

    public void Show(bool value, float duration)
    {
        if (value)
        {
            transform.DOLocalMove(new Vector3(originPos.x, originPos.y, 0), duration + speed).SetEase(outEase);
            Tween t = DOTween.To(() => _canvasGroup.alpha, alpha => { _canvasGroup.alpha = alpha; }, 0.9f, duration + speed).SetEase(outEase);
        }
        else
        {
            transform.DOLocalMove(new Vector3(originPos.x + - 1080, originPos.y + - 540, 0), duration * 0.7f + speed).SetEase(inEase);
            Tween t = DOTween.To(() => _canvasGroup.alpha, alpha => { _canvasGroup.alpha = alpha; }, 0.1f, duration * 0.7f + speed).SetEase(inEase);
        }
    }
}
