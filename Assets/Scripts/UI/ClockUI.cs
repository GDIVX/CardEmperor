using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [TabGroup("Indicator")]
    public float time = 1;
    [TabGroup("Indicator")]
    public LeanTweenType easeType;
    [TabGroup("Events")]
    public Sprite eventIcon;
    [TabGroup("Events")]
    public Color positiveColor,NegativeColor;
    [TabGroup("Events")]
    public float eventIconPadding = 23f;
    //The position of the transform in the array correlate to the time it is representing
    Transform[] _suns;
    GameObject _indicator;
    Transform _eventIconsContainer;


    [Range(0,6)]
    public int debugCurrentTime;

    void Awake()
    {
        _suns = GetChildren(1);
        _indicator = transform.GetChild(2).gameObject;
        _eventIconsContainer = transform.GetChild(0);

        Reset();
    }

    private Transform[] GetChildren(int index)
    {
        Transform parent = transform.GetChild(index);
        List<Transform> res = new List<Transform>();

        foreach (Transform child in parent)
        {
            res.Add(child);
        }

        return res.ToArray();
    }

    public void CreateEventIcon(int timeIndex , bool isPositive ){
        //warp the index between 1 and 4
        timeIndex = timeIndex > 4 ? 0 : (timeIndex < 0 ? 4 : timeIndex);
        Vector2 position = _suns[timeIndex].position;
        position.x += isPositive ? eventIconPadding : -eventIconPadding;

        GameObject iconGameObject = GetDisabledChild(_eventIconsContainer);
        iconGameObject.GetComponent<Image>().color = isPositive ? positiveColor : NegativeColor;

        iconGameObject.transform.position = position;
        iconGameObject.SetActive(true);
    }

    [Button]
    public void Reset()
    {
        foreach (Transform child in _eventIconsContainer)
        {
            child.gameObject.SetActive(false);
        }

        MoveTo(0);
    }

    public void Move(bool forward , int currentTime){
        int index = forward ? currentTime + 1 : currentTime - 1;
        MoveTo(index);
    }

    public void MoveTo(int timeIndex){
        //warp the index between 0 and 4
        timeIndex = timeIndex > 5 ? 0 : (timeIndex < 0 ? 5 : timeIndex);

        //TODO remove debug
        debugCurrentTime = timeIndex;

        Vector2 position = _suns[timeIndex].position;

        //Move the indicator to the new position
        LeanTween.moveY(_indicator , position.y , time).setEase(easeType);
    }

    [Button("Move Up")]
    public void debugMoveUp(){
        Move(true , debugCurrentTime);
    }
    [Button("Move Down")]
    public void debugMoveDown(){
        Move(false , debugCurrentTime);
    }
    [Button]
    public void debugPositiveEvent(){
        CreateEventIcon(debugCurrentTime , true);
    }
    [Button]
    public void debugNegetiveEvent(){
        CreateEventIcon(debugCurrentTime , false);
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
