using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    [ShowInInspector]
    public List<DebugCommand> commandList;

    public bool showConsole;

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
    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Creatures")]
    private Creature creature;
    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Creatures")]
    private CreatureAgent agent;
    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Creatures")]
    private CreatureDisplayer CreatureDisplayer;
    [ShowInInspector]
    [TabGroup("Creatures")]
    int creatureID;
    [Button("Find")]
    [TabGroup("Creatures")]
    void FindCreature(){
        if(creatureID == 0){
            creature = null;
            CreatureDisplayer = null;
            return;
        }

        creature = Creature.GetCreature(creatureID);
        CreatureDisplayer = CreatureDisplayer.GetCreatureDisplayer(creatureID);

        if(CreatureAgent.AgentExist(creatureID)){
            agent = CreatureAgent.GetAgent(creatureID);
            agent.debug_currentState = agent.state.ToString();
        }
        else{
            agent = null;
        }
    }
    
}

