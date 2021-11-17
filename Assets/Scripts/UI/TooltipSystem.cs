using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public GameObject tooltipPrefab;
    void Awake()
    {
        current = this;
    }

    public static void Show(Transform container, string header ="" , string content = "" ){
        GameObject toolTipGameObject = current.GetToolTip(container);

        toolTipGameObject.gameObject.SetActive(true);
        Tooltip tooltip = toolTipGameObject.GetComponent<Tooltip>();
        tooltip.SetText(header , content);
        LeanTween.alpha(toolTipGameObject , .5f , 1);
    }

    private GameObject GetToolTip(Transform container)
    {
        GameObject tooltip = null;
        if(GetDisabledChildrenCount(container) <= 0){
            tooltip = Instantiate(tooltipPrefab , container.transform.position , container.transform.rotation);
            tooltip.transform.SetParent(container);
        }
        else{
            tooltip = GetDisabledChild(container);
        }
        return tooltip;
        
    }

    public static void Hide(Transform container){
        if(current.GetDisabledChildrenCount(container) >= container.childCount) return;

        foreach (Transform child in container)
        {
            LeanTween.alpha(child.gameObject , 0f , 1 );
            child.gameObject.SetActive(false);
        }
    }

    int GetDisabledChildrenCount(Transform t){
        int res = 0;
        for (var i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            res = child.gameObject.activeInHierarchy ? res : res+1; 
        }
        return res;
    }

    GameObject GetDisabledChild(Transform t){
        for (var i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            if(child.gameObject.activeInHierarchy == false){
                return child.gameObject;
            }
        }
        return null;
    }
}
