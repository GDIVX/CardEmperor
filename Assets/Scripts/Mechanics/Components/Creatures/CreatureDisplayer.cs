using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void ShowDisplay(){
        if(ID == 0){
            Debug.LogError("no creature is set to displayer");
            return;
        }

        Creature creature = Creature.GetCreature(ID);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = creature.icon;
        transform.GetChild(1).GetComponent<TextMeshPro>().text = creature.Hitpoint.ToString();
        transform.GetChild(2).GetComponent<TextMeshPro>().text = creature.Armor.ToString();
        transform.GetChild(3).GetComponent<TextMeshPro>().text = creature.Attack.ToString();
        transform.GetChild(4).GetComponent<TextMeshPro>().text = creature.Speed.ToString();

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

        //TODO activate some shader
        transform.localScale = new Vector3(1.2f , 1.2f , 1.2f);
    }

    public void OnDeselect()
    {
        GameManager.CurrentSelected = null;

        //TODO activate some shader
        transform.localScale = new Vector3(1 , 1 , 1);
        
    }
}
