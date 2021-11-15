using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler , IPointerExitHandler 
{
    public List<string> headers = new List<string>(), contents = new List<string>();
    static LTDescr delay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(.5f , () =>{

            string header = null, content = null;

            for (var i = 0; i < headers.Count; i++)
            {
                header = headers.Count > 0 ? headers[i] : null;
                content = contents.Count > 0 ? contents[i] : null;
                TooltipSystem.Show(header,content);   
                
            }
        });

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
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
