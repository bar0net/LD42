using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overlord : MonoBehaviour {
    // DATA STRUCTURES
    public enum EventTypes { NONE, SENTINEL_FIGHT, MINIBOSS_FIGHT, BOSS_FIGHT, TREASURE, MISTERY, MERCHANT }

    [System.Serializable]
    public struct FightingEvent
    {
        public int minLevel;
        public int maxLevel;
        public EnemyData[] enemies;
    }

    // Singleton
    public static Overlord _instance;


    [Header("Enemy Parameters")]
    public int enemyLevel = 1;
    public FightingEvent[] sentinelFights;
    public FightingEvent[] bossFights;
    public FightingEvent[] MiniBossFights;

    [Header("Card Management")]
    // List of playable cards owned by the player
    public List<Card> collection;

    // How many cards should be dealt wech turn
    public int cardsToDeal = 5;

    [Header("Inventory Management")]
    public int inventorySlots = 7;
    public List<Item> inventory;

    [HideInInspector]
    public EnemyData[] enemies;

    System.Random rng;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else DestroyImmediate(gameObject);

        rng = new System.Random();

        // If there are items in the inventory, add Cards to the collection if necessary.
        foreach (Item item in inventory)
        {
            for (int i = 0; i < item.cards.Length; i++) collection.Add(item.cards[i]);
        }
    }

    // Select the enemies for a fight
    public EnemyData[] LoadFight(FightingEvent[] fightList)
    {
        List<FightingEvent> fights = new List<FightingEvent>();

        foreach (FightingEvent f in fightList)
        {
            if (enemyLevel < f.minLevel || enemyLevel > f.maxLevel) continue;
            fights.Add(f);
        }

        if (fights.Count <= 0) return sentinelFights[rng.Next(sentinelFights.Length)].enemies;
        else return fights[rng.Next(fights.Count)].enemies;
    }


    // Manage Events
    public void LoadEvent(EventTypes type)
    {
        switch (type)
        {
            case EventTypes.BOSS_FIGHT:
                enemies = LoadFight(bossFights);
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.MERCHANT:
                break;

            case EventTypes.MINIBOSS_FIGHT:
                enemies = LoadFight(MiniBossFights);
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.MISTERY:
                break;

            case EventTypes.SENTINEL_FIGHT:
                enemies = LoadFight(sentinelFights);
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.TREASURE:
                break;
        }
    }

    public void InventoryAdd(Item item)
    {
        if (inventory.Count < inventorySlots)
        {
            for (int i = 0; i < item.cards.Length; i++) collection.Add(item.cards[i]);
        }
        else
        {
            LoseItem(item);
        }
    }

    public void InventoryRemove(Item item)
    {
        LoseItem(item);
        inventory.Remove(item);
    }

    void LoseItem(Item item)
    {

    }

}
