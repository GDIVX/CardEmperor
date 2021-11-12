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
    bool hasMovedAwayFromOrigin , inFocus;

    [Button(ButtonSizes.Medium)]
    [TabGroup("Move")]

    void Awake()
    {
        originalScale = transform.localScale;
        origin = transform.localPosition;
    }
    public void Move(){
        if(inFocus ) {return;}


        if(hasMovedAwayFromOrigin){
            LeanTween.moveLocal(gameObject , origin , time).setEase(easeType);
        }else{
            LeanTween.moveLocal(gameObject , targetMovePosition , time).setEase(easeType);
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
        if(!inFocus){
            LeanTween.moveLocal(gameObject , targetScalePosition , time).setEase(easeType);
            LeanTween.scale(gameObject , targetScale , time).setEase(easeType);
            inFocus = true;
        }
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Focus")]
    public void UnfocusedOn(){
        if(inFocus){
            LeanTween.moveLocal(gameObject , origin , time).setEase(easeType);
            LeanTween.scale(gameObject , originalScale , time).setEase(easeType);
            inFocus = false;
        }
    }

}
