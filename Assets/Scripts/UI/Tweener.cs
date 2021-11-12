using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    public float time = 1;
    public LeanTweenType easeType;

    [TabGroup("Move")]
    public Vector3 targetMovePosition;
    [TabGroup("Focus")]
    public Vector3 targetScale;
    [TabGroup("Focus")]
    public Vector3 targetScalePosition;

    Vector3 origin , originalScale;
    bool hasMovedAwayFromOrigin , inFocus , isOperating;

    [Button(ButtonSizes.Medium)]
    [TabGroup("Move")]

    void Awake()
    {
        originalScale = transform.localScale;
        origin = transform.localPosition;
    }
    public void Move(){
        if(inFocus || isOperating) {return;}


        if(hasMovedAwayFromOrigin){
            LeanTween.moveLocal(gameObject , origin , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
        }else{
            LeanTween.moveLocal(gameObject , targetMovePosition , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
        }

        hasMovedAwayFromOrigin = !hasMovedAwayFromOrigin;
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Move")]
    public void MoveAwayFromOrigin(){
        if(!hasMovedAwayFromOrigin)
            Move();
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Move")]
    public void MoveToOrigin(){
        if(hasMovedAwayFromOrigin)
            Move();
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Focus")]
    public void FocusOn(){
        if(!inFocus && !isOperating){
            LeanTween.moveLocal(gameObject , targetScalePosition , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
            LeanTween.scale(gameObject , targetScale , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
            inFocus = true;
        }
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Focus")]
    public void UnfocusedOn(){
        if(inFocus && !isOperating){
            LeanTween.moveLocal(gameObject , origin , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
            LeanTween.scale(gameObject , originalScale , time).setEase(easeType).setOnStart(OnWorking).setOnComplete(OnComplete);
            inFocus = false;
        }
    }

    void OnComplete(){
        isOperating = false;
    }
        
    void OnWorking() => isOperating = true;

}
