using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(menuName = "Invasion Waves /Enemy Invasion Selection")]
    public class EnemiesInvasionSelection : ScriptableObject
    {
        public List<EnemiesInvasion> easy;
        public List<EnemiesInvasion> avarage;
        public List<EnemiesInvasion> hard;
    }
}