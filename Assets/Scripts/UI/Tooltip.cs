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
