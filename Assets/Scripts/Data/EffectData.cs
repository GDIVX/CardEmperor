using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect GUI Data")]
public class EffectData : ScriptableObject
{
    public Sprite icon;
    public Color color = Color.white;
    public string toolTipHeader;
    [TextArea]
    public string tooltipDescription;
}
