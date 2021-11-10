using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class DebugCommandController : MonoBehaviour
{
    public List<object> commandList;

    #region Commands
        public static DebugCommand<string> ADD_CARD;
        public static DebugCommand GET_DISTANCE;
        public static DebugCommand GET_POSITION;
        public static DebugCommand SMILE;
        public static DebugCommand HELP;
    #endregion


    bool showConsole;
    bool showHelp;
    string input;
    Vector2 scroll;

    void Awake()
    {
        SMILE = new DebugCommand("smile" , "Have a nice day" , "smile" , () => {
            Debug.Log(":)");
        });

        ADD_CARD = new DebugCommand<string>("add card" ,"Add a new card to your hand", "add_card <card_name>", (x) =>{
            Card card = Card.BuildCard( x , Player.Main.ID);
            CardsMannager.Instance.hand.AddCard(card);
        } );

        HELP = new DebugCommand("help" , "Show a list of commands" , "help", () =>{
            showHelp = true;
        });

        GET_DISTANCE = new DebugCommand("get_distance" , "Get the distance from this creature to map origin point" , "get_distance" , ()=>{
            Creature creature = GameManager.CurrentSelected as Creature;
            if(creature != null){
                int distance = WorldController.DistanceOf(creature.position , Vector3Int.zero);
                Debug.Log($"The distance of the currently selected creature to origin is {distance.ToString()}");
            }
        });
        GET_POSITION = new DebugCommand("get_position" , "Get the position of this creature." , "get_position" , ()=>{
            Creature creature = GameManager.CurrentSelected as Creature;
            if(creature != null){
                Debug.Log($"The position of the currently selected creature is {creature.position.ToString()}");
            }
        });

        commandList = new List<object>{
            SMILE,
            ADD_CARD,
            GET_DISTANCE,
            HELP
        };
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5)){
            showConsole = !showConsole;
        }
        if (Input.GetKeyDown(KeyCode.F4) && showConsole)
        {
            HandleInput();
            input ="";
        }
    }

    void OnGUI()
    {
        if(!showConsole){return;}

        float y = 0f;

        if(showHelp){
            Debug.Log("?");
            GUI.Box(new Rect(0, y , Screen.width , 100), "");
            Rect viewport = new Rect(0,0,Screen.width - 30 , 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y+5f, Screen.width, 90) , scroll , viewport);

            for (var i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5 , 20 * i , viewport.width - 100 , 20);
                GUI.Label(labelRect , label);

                GUI.EndScrollView();
            }

            y+=100;
        }

        GUI.Box(new Rect(0,y,Screen.width , 30) , "");
        GUI.backgroundColor = new Color(0,0,0,0);
        input = GUI.TextField(new Rect(10f,y + 5f , Screen.width - 20f , 20f) ,input);
    }

    void HandleInput(){
        
        string[] properties = input.Split(' ');

        for (var i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandID)){
                if(commandList[i] as DebugCommand != null){
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
            else
            {
                if(commandList[i] as DebugCommand<string> != null){
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
            }
        }
    }
}
