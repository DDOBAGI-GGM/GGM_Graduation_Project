using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowRecipeOnlyOne : MonoBehaviour
{
    private CanvasGroup canvas;
    [SerializeField] private RectTransform recipeImage;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        recipeImage.DOLocalMoveX(0, 1.5f).SetEase(Ease.OutCubic).OnComplete(() => Fade()) ;
    }

    private void Fade()
    {
        canvas.DOFade(0, 1f);
    }
}
