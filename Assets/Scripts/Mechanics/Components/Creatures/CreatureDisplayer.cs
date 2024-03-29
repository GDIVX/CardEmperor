using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.UI;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public class CreatureDisplayer : MonoBehaviour , IClickable
{
    static Dictionary<int , CreatureDisplayer> regestry = new Dictionary<int, CreatureDisplayer>();
    public int ID;

    public static GameObject Create(Creature creature , Vector3 position){
        GameObject displayGameObject = WorldController.Instance.CreatureTemplate;

        displayGameObject = GameObject.Instantiate(displayGameObject , position , Quaternion.identity);
        CreatureDisplayer displayer = displayGameObject.GetComponent<CreatureDisplayer>();

        displayer.ID = creature.ID;
        regestry.Add(displayer.ID , displayer);
        return displayGameObject;
    }



    public void SetDisplay(bool isActive){
        if(isActive){
            ShowDisplay();
        }
        else{
            gameObject.SetActive(false);
        }
    }

    public void AttackAnimation(Vector3Int target){
        var worldPosition = WorldController.Instance.map.CellToWorld(target);
        transform.LeanMove(worldPosition , .8f).setEase(LeanTweenType.easeInExpo).setLoopPingPong(1);
    }

    public void DamagedAnimation(){
        transform.LeanMoveLocal(((Vector3)Random.insideUnitCircle *0.8f) + transform.position , .8f).setEase(LeanTweenType.easeInElastic).setLoopPingPong(1);
    }

    internal void BlockedAnimation()
    {
        transform.LeanMoveLocalY(transform.position.y + .5f, 0.25f).setEaseInElastic().setEaseOutBounce().setLoopPingPong(1);
    }

    void ShowDisplay(){
        if(ID == 0){
            Debug.LogError("no creature is set to displayer");
            return;
        }

        Creature creature = Creature.GetCreature(ID);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = creature.icon;

        transform.name = Card.GetCard(ID).data.cardName;

        gameObject.SetActive(true);
    }

    public static CreatureDisplayer GetCreatureDisplayer(int ID){
        return regestry[ID];
    }

    public void OnLeftClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnRightClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnSelect()
    {
        IClickable selected = GameManager.CurrentSelected;
        selected.OnDeselect();
        GameManager.CurrentSelected = this;

        var interactionTable = BoardInteractionMatrix.GetInteractionTable(Creature.GetCreature(ID));
        WorldController.Instance.overlayController.PaintTheMap(interactionTable);
    }

    public void OnDeselect()
    {
        GameManager.CurrentSelected = null;

        WorldController.Instance.overlayController.Clear();
        
    }

    public void Toast(string msg , float time ,float fontSize = 30 ){
        Prompt.Toast(msg , transform.position , time , fontSize);
    }

    void OnMouseEnter()
    {
        CreatureInspector.ShowCreatureCard(ID);
        transform.LeanScale(new Vector3(1.2f , 1.2f , 1.2f) , .2f).setEase(LeanTweenType.easeInOutSine);
    }

    void  OnMouseExit()
    {
        if(GameManager.CurrentSelected != this)
            CreatureInspector.Hide();

        transform.LeanScale(new Vector3(1 , 1 , 1) , .2f).setEase(LeanTweenType.easeInOutSine);
    }

    internal void UpdatePosition(Vector3Int newPosition)
    {
        var worldPosition = WorldController.Instance.map.CellToWorld(newPosition);
        transform.LeanMove(worldPosition , 1).setEase(LeanTweenType.easeInSine);
        var interactionTable = BoardInteractionMatrix.GetInteractionTable(Creature.GetCreature(ID));
        WorldController.Instance.overlayController.PaintTheMap(interactionTable);

    }

}
