using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable 
{
    public void OnLeftClick();
    public void OnRightClick();
    public void OnSelect();
    public void OnDeselect();

}
