using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overlord : MonoBehaviour {
    public const float _SELL_RATIO_ = 1.2f;

    // DATA STRUCTURES
    public enum EventTypes { NONE, SENTINEL_FIGHT, MINIBOSS_FIGHT, BOSS_FIGHT, TREASURE, MISTERY, MERCHANT }

    [System.Serializable]
    public class EventData
    {
        public EventTypes type;
        public int minLevel;
        public int maxLevel;
        public EnemyData[] enemies;
        public Item[] rewards;
    }

    // Singleton
    public static Overlord _instance;

    // Player Stats
    [HideInInspector] public int enemiesKilled = 0;
    [HideInInspector] public int eventsCompleted = 0;
    [HideInInspector] public int itemsCollected = 0;

    public int enemyLevel = 1;
    [Header("Event Types")]
    public EventData[] sentinelFights;
    public EventData[] bossFights;
    public EventData[] MiniBossFights;
    public EventData[] merchantEvent;
    public EventData[] treasureEvent;
    public EventData[] misteryEvent;

    [Header("Card Management")]
    // List of playable cards owned by the player
    public List<Card> collection;

    // How many cards should be dealt wech turn
    public int cardsToDeal = 5;

    [Header("Inventory Management")]
    public int inventorySlots = 7;
    public List<Item> inventory;

    //public EnemyData[] enemies;
    [SerializeField]
    public EventData currentEvent = null;

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
        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];
            for (int j = 0; j < item.cards.Length; j++) collection.Add(item.cards[j]);
        }

        currentEvent = null;
    }

    // Select the enemies for a fight
    public EventData CreateEvent(EventData[] eventList)
    {
        if (eventList.Length == 0) return null;

        List<EventData> events = new List<EventData>();

        foreach (EventData f in eventList)
        {
            if (enemyLevel < f.minLevel || enemyLevel > f.maxLevel) continue;
            events.Add(f);
        }

        if (events.Count <= 0) return eventList[rng.Next(eventList.Length)];
        else return events[rng.Next(events.Count)];
    }


    // Manage Events
    public void LoadEvent(EventData data)
    {
        if (data == null) return;
        currentEvent = data;

        switch (data.type)
        {
            case EventTypes.BOSS_FIGHT:
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.MERCHANT:
                SceneManager.LoadScene("Merchant");
                break;

            case EventTypes.MINIBOSS_FIGHT:
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.MISTERY:
                SceneManager.LoadScene("Mistery");
                break;

            case EventTypes.SENTINEL_FIGHT:
                SceneManager.LoadScene("Fight");
                break;

            case EventTypes.TREASURE:
                SceneManager.LoadScene("Treasure");
                break;
        }
    }

    public void InventoryAdd(Item item)
    {
        if (inventory.Count < inventorySlots)
        {
            itemsCollected += 1;
            inventory.Add(item);
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

    public void LoseItem(Item item)
    {
        enemyLevel += item.cost;
    }

    public EventData GetRandomEvent(EventTypes type)
    {
        switch (type)
        {
            case EventTypes.BOSS_FIGHT:
                return CreateEvent(bossFights);

            case EventTypes.MERCHANT:
                return CreateEvent(merchantEvent);

            case EventTypes.MINIBOSS_FIGHT:
                return CreateEvent(MiniBossFights);

            case EventTypes.MISTERY:
                return CreateEvent(misteryEvent);

            case EventTypes.SENTINEL_FIGHT:
                return CreateEvent(sentinelFights);

            case EventTypes.TREASURE:
                return CreateEvent(treasureEvent);
        }

        return null;
    }
}
