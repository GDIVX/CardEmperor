using System.Net.Mime;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vector3 = UnityEngine.Vector3;
using Sirenix.OdinInspector;
using Vector2 = UnityEngine.Vector2;
using System;

namespace Assets.Scripts.UI
{
    public class Prompt : MonoBehaviour
    {
        
        private static Prompt Main;
        public LeanTweenType ease; 
        
        void Awake()
        {
            if(Main == null){
                Main = this;
            }
        }

        public static void Toast(string massage , Vector3 worldPosition , float time , float fontSize = 30f){

            GameObject display = GetDisplay();
            TextMeshProUGUI text = display.GetComponent<TextMeshProUGUI>();
            Prompt prompt = display.GetComponent<Prompt>();
            display.SetActive(true);

            display.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
            text.text = massage;
            text.fontSize = fontSize;

            LeanTween.value(display , text.alpha , 0 , time).setEase(Main.ease).setOnUpdate(prompt.SetAlpha).setOnComplete(prompt.Rest);
        }

        public void Rest()
        {
            if(Main == this) return;
            gameObject.SetActive(false);
        }

        private static GameObject GetDisplay()
        {
            GameObject display;
            if(UIController.GetDisabledChildrenCount(Main.transform) > 0){
                display =  UIController.GetDisabledChild(Main.transform);
                display.GetComponent<TextMeshProUGUI>().alpha = 225;
                return display;
            }

            display = new GameObject();
            var text = display.AddComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Center;
            text.alignment = TextAlignmentOptions.Midline;
            text.enableWordWrapping = false;
            display.AddComponent<Prompt>();

            display = GameObject.Instantiate(display);
            display.transform.SetParent(Main.transform);

            return display;
        }


        public void SetAlpha(float value){
            GetComponent<TextMeshProUGUI>().alpha = value;
        }

        [Button]
        public void Test(){
            Toast("<b>Toast !</b> <color=green>Nice!</color>" , new Vector3(14,7,0) ,1f , 24f); 
        }
    }
}        
    