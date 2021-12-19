using System.Diagnostics;
using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Vector3 = UnityEngine.Vector3;
using Assets.Scripts.Mechanics.Systems.Players;
using Debug = UnityEngine.Debug;
using Assets.Scripts.Mechanics.Components.Effects;

[System.Serializable]
public class Creature : IClickable
{

    public Sprite icon { get => _icon; }


    public Vector3Int position { get => _position; }
    public bool isAlive{get =>_isAlive;}
    public int ID { get => _ID; }
    public int PlayerID { get => _PlayerID; }

    public int movement { get => _movement; }
    public Player Player { get { return Player.GetPlayer(PlayerID); } }
    public bool flying { get { return _flying; } }

    public int attacksPerTurn = 1;

    public int damageBonus;

    public Dictionary<Type, Effect> effects = new Dictionary<Type, Effect>();


    public static Action<Creature> OnCreatureDeath , OnTurnEnds;


    static Dictionary<int, Creature> creaturesRegestry = new Dictionary<int, Creature>();

    public int hitpoints, armor, attack, speed, attackRange, attacksAttempts;
    [ShowInInspector]
    [ReadOnly]
    int _ID, _PlayerID, _movement;
    [ShowInInspector]
    [ReadOnly]
    Sprite _icon;

    [ShowInInspector]
    [ReadOnly]
    Vector3Int _position;
    [ShowInInspector]
    [ReadOnly]
    private bool  _flying , _isAlive;
    [ShowInInspector]
    [ReadOnly]
    CreatureAbility ability;

    public Creature(CreatureData data, int cardID, Vector3Int position)
    {
        this.hitpoints = data.hitpoint;
        this.armor = data.armor;
        this.attack = data.attack;
        this.speed = data.speed;
        this.attackRange = data.attackRange;
        this._icon = data.icon;
        _position = position;
        _movement = speed;
        
        _isAlive = true;
        _flying = data.flying;

        _ID = cardID;
        Card card = Card.GetCard(cardID);
        _PlayerID = card.playerID;
        creaturesRegestry.Add(cardID, this);
        WorldController.Instance.world[position.x, position.y].CreatureID = cardID;
        ApplyTileBonuses();

        if (data.abilityScriptName != null && data.abilityScriptName != "")
        {
            ability = System.Activator.CreateInstance(System.Type.GetType(data.abilityScriptName), cardID) as CreatureAbility;
        }

        GameManager.Instance.turnSequenceMannager.OnTurnStart += OnTurnStart;
        GameManager.Instance.turnSequenceMannager.OnTurnComplete += OnTurnEnd;
    }

    public static Creature BuildCreatureCardless(string creatureCardName, int playerID, Vector3Int position)
    {
        Card tempCard = Card.BuildCard(creatureCardName, playerID);
        return new Creature(tempCard.data.creatureData, tempCard.ID, position);
    }

    public static Creature BuildAndSpawnCardless(string creatureCardName, int playerID, Vector3Int position)
    {
        Creature creature = BuildCreatureCardless(creatureCardName, playerID, position);

        var worldPosition = WorldController.Instance.map.GetCellCenterLocal(position);
        GameObject _gameObject = CreatureDisplayer.Create(creature, worldPosition);
        CreatureDisplayer displayer = _gameObject.GetComponent<CreatureDisplayer>();

        displayer.SetDisplay(true);

        return creature;
    }

    public void Kill()
    {
        OnCreatureDeath?.Invoke(this);
        //Clear view
        GameObject.Destroy(CreatureDisplayer.GetCreatureDisplayer(ID).gameObject);
        //Move the card into exile
        if(PlayerID == Player.Main.ID)
            CardsMannager.Instance.exilePile.Drop(Card.GetCard(ID));
        //Remove the creature from the board
        WorldTile tile = WorldController.Instance.world[position.x, position.y];
        tile.CreatureID = 0;
        _isAlive = false;
    }

    public void AddEffect(Effect effect)
    {
        if(!isAlive) return;

        if (effects.ContainsKey(effect.GetType()))
        {
            effects[effect.GetType()].value += effect.value;
            effects[effect.GetType()].OnCreated();
            return;
        }

        effects.Add(effect.GetType(), effect);
        effect.SetCreature(ID);
        effect.OnCreated();
    }
    public void SetEffect(Effect effect , bool allowBiggerValues = true)
    {
        if(!isAlive) return;

        if (effects.ContainsKey(effect.GetType()))
        {
            if(allowBiggerValues == true && effects[effect.GetType()].value >= effect.value){
                return;
            }
            effects[effect.GetType()].value = effect.value;
            effects[effect.GetType()].OnCreated();
            return;
        }

        effects.Add(effect.GetType(), effect);
        effect.SetCreature(ID);
        effect.OnCreated();
    }
    internal void TakeDamage(int damage)
    {
        if(!isAlive) return;

        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            Kill();
        }
    }

    internal void OnAttackBlocked(int damage)
    {
        if(!isAlive) return;
        armor--;
        CreatureDisplayer.GetCreatureDisplayer(ID).BlockedAnimation();
    }

    internal void OnAttackPassed(int damage)
    {
        CreatureDisplayer.GetCreatureDisplayer(ID).DamagedAnimation();

    }

    public static Creature GetCreature(int ID)
    {
        if (CreatureExist(ID) == false)
        {
            UnityEngine.Debug.LogError($"Trying to get a creature that dose not exist with ID {ID}");
            return null;
        }
        return creaturesRegestry[ID];
    }

    public static bool CreatureExist(int ID)
    {
        return creaturesRegestry.ContainsKey(ID);
    }

    public void OnLeftClick()
    {
        UnityEngine.Debug.Log("How did you get here?");
    }

    public void OnRightClick()
    {

    }

    public void OnSelect()
    {
        GameManager.CurrentSelected = this;
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        displayer.OnSelect();
    }

    public void OnDeselect()
    {
        GameManager.CurrentSelected = null;
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        displayer.OnDeselect();
    }

    public void InteractWithCreature(Creature creature)
    {
        if (ability == null || !isAlive || !creature.isAlive) { return; }
        int distanceToCreature = WorldController.DistanceOf(position , creature.position);
        if(distanceToCreature > attackRange) return;

        if (creature.Player == Player)
        {
            ability.ActionOnFriendlyCreature(creature);
        }
        else if (attacksPerTurn > attacksAttempts)
        {
            attacksAttempts++;
            ability.ActionOnEnemyCreature(creature);
            CreatureDisplayer.GetCreatureDisplayer(ID).AttackAnimation(creature.position);
            var interactionTable = BoardInteractionMatrix.GetInteractionTable(this);
            WorldController.Instance.overlayController.PaintTheMap(interactionTable);
        }
    }


    public void MoveTo(Vector3Int target)
    {
        if(!isAlive) return;

        int distance = WorldController.DistanceOf(position, target);
        if (distance <= _movement && (flying || WorldController.Instance.world[target.x, target.y].walkable))
        {
            _movement -= distance;
            UpdatePosition(target);
        }
    }

    public override string ToString()
    {
        Card card = Card.GetCard(ID);
        if (card != null)
        {
            return card.data.ToString();
        }
        return null;
    }

    public void UpdatePosition(Vector3Int newPosition)
    {
        if(!isAlive) return;
        //Clean old tile
        WorldTile tile = WorldController.Instance.world[position.x, position.y];
        RemoveTileBonuses();
        tile.CreatureID = 0;

        //Set Creature ID to new tile
        tile = WorldController.Instance.world[newPosition.x, newPosition.y];
        tile.CreatureID = ID;

        //Move the displayer
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);

        _position = newPosition;
        displayer.UpdatePosition(newPosition);

        ApplyTileBonuses();
    }

    private void RemoveTileBonuses()
    {
        WorldTile tile = WorldController.Instance.GetTile(position);

        attack -= tile.armorBonus;
        armor -= tile.armorBonus;
    }

    private void ApplyTileBonuses()
    {
        WorldTile tile = WorldController.Instance.GetTile(position);
        
        attack += tile.attackBonus;
        armor += tile.armorBonus;
    }

    public void ToastAttackFormated(int damage, int targetHitpoint)
    {
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);

        if (damage <= 0)
        {
            //Missed!
            displayer.Toast($"<color=orange><b><i>Blocked!</color></b></I>", 1,50);
            return;
        }

        float damageRelativeToHitpoints = damage / targetHitpoint;
        float fontSize = Mathf.Clamp(35 * damageRelativeToHitpoints, 24, 50);

        displayer.Toast($"<color=red><b><i>{damage}</color></b></I>", 1, fontSize);


    }

    public void RemoveEffect(Effect effect)
    {
        effects.Remove(effect.GetType());
    }

    void OnTurnStart(Turn turn)
    {
        if (turn == null || turn.player == null || !isAlive)
        {
            return;
        }
        if (turn.player == this.Player)
        {
            _movement = speed;
            attacksAttempts = 0;
        }
    }

    void OnTurnEnd(Turn turn)
    {
        if (turn == null || turn.player == null || !isAlive)
        {
            return;
        }
        if (turn.player == this.Player)
        {
            var effectsCopy = new Dictionary<Type, Effect>(effects);

            foreach (var typeEffectPair in effectsCopy)
            {
                typeEffectPair.Value.OnTurnEnd();
            }
        }

    }

    internal static Creature GetCreatureByPosition(Vector3Int targetPosition)
    {
        if (!WorldController.Instance.IsTileExist(targetPosition))
        {
            return null;
        }
        WorldTile tile = WorldController.Instance.world[targetPosition.x, targetPosition.y];
        if (tile.CreatureID == 0)
        {
            return null;
        }
        return Creature.GetCreature(tile.CreatureID);
    }
    internal static Creature[] GetAll(int playerID)
    {
        List<Creature> res = new List<Creature>();
        List<Creature> regestry = new List<Creature>(creaturesRegestry.Values);

        foreach (Creature creature in regestry)
        {
            if (creature.PlayerID == playerID)
            {
                res.Add(creature);
            }
        }

        return res.ToArray();
    }
}
