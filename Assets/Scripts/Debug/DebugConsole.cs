using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public List<DebugCommand> commandList;

    bool showConsole;

    DebugCommand SummonMonster;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5)){
            showConsole = !showConsole;
        }
    }

    void Awake()
    {
        SummonMonster = new DebugCommand("Summon monster" , ()=>{
            Card card = Card.BuildCard("Archer" , Player.Rival.ID);
            CardData data = card.data;
            Vector3Int cellPosition = WorldController.Instance.GetRandomTile();
            Vector3 position = WorldController.Instance.map.CellToWorld(cellPosition);
            Creature creature = new Creature(data.creatureData , card.ID , cellPosition);
            
            GameObject _gameObject = CreatureDisplayer.Create(creature , position);
            
            
            CreatureDisplayer displayer = _gameObject.GetComponent<CreatureDisplayer>();

            displayer.SetDisplay(true);
        });

        commandList = new List<DebugCommand>{
            SummonMonster
        };
    }

    Vector2 scroll;
    void OnGUI()
    {
        if(!showConsole) {return;}

        GUI.Box(new Rect(5 , 5 , Screen.width  - 20, (commandList.Count * 55) + 10  ) , "");
        GUI.backgroundColor = Color.black;
        Rect viewpot = new Rect(0,0 , Screen.width  - 20 , (commandList.Count * 55) + 10);
        
        scroll = GUI.BeginScrollView(new Rect(5 , 5 , Screen.width  - 20 , 90) , scroll , viewpot );
        
        for (var i = 0; i < commandList.Count; i++)
        {
            DebugCommand command = commandList[i];
            string name = command.name;

            if(GUI.Button(new Rect(10 * i + 10 , 10 * i + 10 , (name.Length * 10) , 50) , name)){
                command.Invoke();
            }
        }

        GUI.EndScrollView();
    }
}
