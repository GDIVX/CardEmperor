using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public abstract class Effect 
    {
        public EffectData UIData;
        public int value;
        public int ID =>_ID;
        public int creatureID;

        private int _ID;

        static Dictionary<int,Effect> regestry = new Dictionary<int, Effect>();

        public Effect(int value){
            this.value = value;
            this._ID = IDFactory.GetUniqueID();
            regestry.Add(_ID , this);
        }

        public void SetCreature(int creatureID){
            this.creatureID = creatureID;
        }

        protected EffectData GetData(string name){
            EffectData data = Resources.Load<EffectData>($"Data/Effects/{name}");
            if(data == null){
                Debug.LogError($"Can't find a effect data/Resources/Effects/{name}");
                return null;
            }

            return data;
        }

        public void Remove(){
            Creature.GetCreature(creatureID).RemoveEffect(this);
            _ID = 0;
            creatureID = 0;
            OnRemoved();
        }

        public bool IsActive(){
            return (_ID != 0 && creatureID != 0);
        }

        public void OnCreated(){
            if(IsActive()){
                _OnCreated();
            }
        }

        public void OnTurnEnd(){
            Debug.Log("!");
            if(IsActive()){
                _OnTurnEnd();
            }
        }

        /// <summary>
        /// Called when the effect is first created
        /// </summary>
        protected abstract void _OnCreated();
        /// <summary>
        /// Called when a turn ends
        /// </summary>
        protected abstract void _OnTurnEnd();
        protected abstract void OnRemoved();

        public static Effect GetEffect(int ID){
            return regestry[ID];
        }
    }
}