using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(menuName = "Invasion Waves /Enemy Waves ")]
    public class EnemiesWave : ScriptableObject
    {
        public List<CardData> enemies;
    }
}