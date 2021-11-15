using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Transform tooltipContainer;
    public GameObject tooltipPrefab;
    public readonly Stack<GameObject> tooltips = new Stack<GameObject>(), disabledTooltips = new Stack<GameObject>();

    void Awake()
    {
        current = this;
    }

    public static void Show(string header ="" , string content = ""){
        GameObject toolTipGameObject = current.GetToolTip();

        toolTipGameObject.gameObject.SetActive(true);
        Tooltip tooltip = toolTipGameObject.GetComponent<Tooltip>();
        tooltip.SetText(header , content);
        LeanTween.alpha(toolTipGameObject , .5f , 1);
    }

    private GameObject GetToolTip()
    {
        GameObject tooltip = null;
        if(disabledTooltips.Count <= 0){
            tooltip = Instantiate(tooltipPrefab , tooltipContainer.transform.position , tooltipContainer.transform.rotation);
            tooltip.transform.SetParent(tooltipContainer);
        }
        else{
            tooltip = disabledTooltips.Pop();
        }
        
        current.tooltips.Push(tooltip);
        return tooltip;
        
    }

    public static void Hide(){
        if(current.tooltips.Count <= 0) return;

        foreach (var tooltip in current.tooltips)
        {
            LeanTween.alpha(tooltip.gameObject , 0f , 1 );
            tooltip.gameObject.SetActive(false);
            current.disabledTooltips.Push(tooltip);
        }
        current.tooltips.Clear();
    }

    public static int DisableTooltipsCount(){
        return current.disabledTooltips.Count;
    }
}
