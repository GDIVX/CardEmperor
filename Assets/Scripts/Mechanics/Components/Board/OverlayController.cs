using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using Assets.Scripts.Mechanics.Systems.Players;

public class OverlayController : MonoBehaviour
{
    public TileBase red, blue;
    Tilemap map;

    void Awake()
    {
        map = GetComponent<Tilemap>();
        GameManager.Instance.turnSequenceMannager.OnTurnStart += OnTurnStart;
    }

    public void PaintTheMap(Dictionary<Vector3Int , string> table){

        map.ClearAllTiles();

        foreach (var position in table)
        {
            switch(position.Value){
                case "Red":
                    map.SetTile(position.Key , red);
                    break;
                default:
                    map.SetTile(position.Key , blue);
                    break;
            }
        }
    }

    public void Clear(){
        map.ClearAllTiles();
    }

    public void OnTurnStart(Turn turn){
        if(turn.player == Player.Main){
            Creature creature = GameManager.CurrentSelected as Creature;
            if(creature == null) return;

            var table = BoardInteractionMatrix.GetInteractionTable(creature);
            PaintTheMap(table);
        }
    }

}
