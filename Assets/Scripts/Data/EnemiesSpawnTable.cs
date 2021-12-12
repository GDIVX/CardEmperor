using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Enemies/Spawn Table")]
public class EnemiesSpawnTable : SerializedScriptableObject
{
    [TableMatrix()]
    public CardData[,] spawnTable = new CardData[9,6];
    
}