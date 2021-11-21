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


    void Awake()
    {
        originalScale = transform.localScale;
        origin = transform.localPosition;
    }
    [Button(ButtonSizes.Medium)]
    [TabGroup("Move")]
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
    public void ScaleFocus(){
        if(!inFocus){
            LeanTween.scale(gameObject , targetScale , time).setEase(easeType);
            inFocus = true;
        }
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Focus")]
    public void UnscaledFocus(){
        if(inFocus){
            LeanTween.scale(gameObject , originalScale , time).setEase(easeType);
            inFocus = false;
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

    [Button(ButtonSizes.Medium)]
    [TabGroup("Showcase")]
    public void Showcase(){
        if(!gameObject.activeInHierarchy){
            if(originalScale == null || originalScale == Vector3.zero){
                originalScale = transform.localScale;
            }

            gameObject.SetActive(true);
            LeanTween.scale(gameObject , originalScale , time).setEase(easeType);
        }
    }

    [Button(ButtonSizes.Medium)]
    [TabGroup("Showcase")]
    public void Hide(){
        if(gameObject.activeInHierarchy){
            originalScale = transform.localScale;

            LeanTween.scale(gameObject  , originalScale * .05f , time).setEase(easeType).setOnComplete(() => gameObject.SetActive(false));
        }
    }

}
