using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler , IPointerExitHandler 
{
    public List<string> headers = new List<string>(), contents = new List<string>();
    public RectTransform anchor;
    public string containerGameObjectName;
    TooltipContainer container;
    static LTDescr delay;

    void Awake()
    {
        container = GameObject.Find(containerGameObjectName).GetComponent<TooltipContainer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(.5f , () =>{

            string header = null, content = null;

            for (var i = 0; i < headers.Count; i++)
            {
                header = headers.Count > 0 ? headers[i] : null;
                content = contents.Count > 0 ? contents[i] : null;
                TooltipSystem.Show(container.transform ,header,content);   
                
            }
        });
        container.SetPosition(anchor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide(container.transform);
    }

    public void SetTextFromCard(KeywordDefinition[] keywords){

        headers.Clear();
        contents.Clear();
        foreach (var keyword in keywords)
        {
            headers.Add(keyword.keyword.ToString());
            contents.Add(GameManager.Instance.definitions.GetKeywordTooltipText(keyword));
        }
    }

}
