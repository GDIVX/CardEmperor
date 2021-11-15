using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWarpLimit;
    public RectTransform rectTransform;
    public float padding = 4f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 position = Input.mousePosition;
        int siblingIndex = transform.GetSiblingIndex();
        int disabledSiblings = TooltipSystem.DisableTooltipsCount();
        float extraPadding = 0;
        if(siblingIndex > 0){
            Tooltip lastTooltip = transform.parent.GetChild(siblingIndex - 1).GetComponent<Tooltip>();
            extraPadding = lastTooltip.rectTransform.rect.height;
        }
        float posY = position.y - (padding * (transform.GetSiblingIndex() - disabledSiblings) + extraPadding);

        position.y += posY;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX,pivotY);
        transform.position = position;
    }

    public void SetText(string header ="" , string content = ""){
        if(string.IsNullOrEmpty(header)){
            headerField.gameObject.SetActive(false);
        }
        else{
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        if(string.IsNullOrEmpty(content)){
            contentField.gameObject.SetActive(false);
        }
        else{
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }

                int headerLength = headerField.text.Length;
        int contentLEngth = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWarpLimit || contentLEngth > characterWarpLimit);
    }
}
