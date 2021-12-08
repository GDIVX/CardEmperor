using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(menuName = "Invasion Waves /Enemies Invasion")]
    public class EnemiesInvasion : ScriptableObject
    {
        public List<EnemiesWave> enemiesWaves;
    }
}