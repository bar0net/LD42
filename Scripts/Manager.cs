using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    const float _ENEMY_SEPARATION_ = 256;

    [Header("Habilities UI Management")]
    public Transform abilityPanel;
    public GameObject costToken;
    public GameObject blankAbility;
    public Collection.TokenColor[] tokenColors;
    public GameObject abilityTooltipPanel;
    public Text abilityTooltipDescription;
    public Text abilityTooltipDamage;
    public Text abilityTooltipMagic;
    public Text abilityTooltipShield;


    [Header("Card UI")]
    public RectTransform handPanel;
    public GameObject cardUI;

    [Header("Enemy UI")]
    public RectTransform enemiesPanel;
    public GameObject enemyPrefab;

    // =================================
    // Cards currently on the draft pile
    [SerializeField]
    List<Card> pile;

    // Cards currently in the discard pile
    [SerializeField]
    List<Card> discard;

    List<AbilityDisplay> habilities = new List<AbilityDisplay>();

    public Dictionary<Collection.TokenTypes, Color> tokenColorsDict = new Dictionary<Collection.TokenTypes, Color>();
    Dictionary<Collection.TokenTypes, int> selectedTokens = new Dictionary<Collection.TokenTypes, int>();

    [HideInInspector]
    public Player player;
    List<Enemy> _enemies = new List<Enemy>();
    public Enemy target;

    Queue<Character> turns = new Queue<Character>();
    Character current;

    Overlord _o;

	// Use this for initialization
	void Start () {
        _o = FindObjectOfType<Overlord>();
        player = FindObjectOfType<Player>();

        // Get and Display Enemies
        if (_o.enemies.Length == 0)
        {
            Debug.LogWarning("[Manager::Start] No enemies in Overlord to populate fight.");
            _o.enemies = _o.LoadFight(_o.sentinelFights);
        }
        for (int i = 0; i < _o.enemies.Length; i++) CreateEnemy(i);

        //player = FindObjectOfType<Player>();
        //foreach (Enemy e in FindObjectsOfType<Enemy>()) _enemies.Add(e);
        SetTarget(_enemies[0]);

        foreach (Enemy e in _enemies) turns.Enqueue(e);
        turns.Enqueue(player);
        
        // Create draft and discard piles
        discard = new List<Card>();
        pile = new List<Card>(_o.collection);
        pile = Collection.Shuffle(pile);

        // Store Token-Color pairs in a dictionary for convenience
        for (int i = 0; i < tokenColors.Length; i++) tokenColorsDict[tokenColors[i].type] = tokenColors[i].value;

        // Initialize selectedTokens with each token at 0
        foreach (Collection.TokenTypes token in System.Enum.GetValues(typeof(Collection.TokenTypes)))
            selectedTokens[token] = 0;

        // Display Information
        DisplayHabilities();
        DisplayInventory();

        // Start the Fight
        turns.Peek().StartTurn();
	}

    // TODO: An animation when you reshuffle the cards
    // TODO: Used cards go to a discard pile instead of banishing.
    void Deal()
    {
        DiscardAll();

        // Deal the required number of cards to hand
        for (int i = 0; i < _o.cardsToDeal; i++)
        {
            // If pile is empty, reshuffle the discard pile and continue
            if (pile.Count == 0)
            {
                pile = Collection.Shuffle(discard);
                discard = new List<Card>();
            }

            // If pile is still empty, all cards are already on hand
            // therefore we cannot deal more cards. Exit the loop
            if (pile.Count == 0) break;

            // Pick the top card of the pile and deal it.
            Card c = pile[0];
            pile.RemoveAt(0);

            GameObject go = Instantiate(cardUI);
            RectTransform rt = go.GetComponent<RectTransform>();
            CardDisplay cd = go.GetComponent<CardDisplay>();

            go.transform.SetParent(handPanel);
            rt.localPosition = - Vector3.right * 160;
            rt.localScale = Vector3.one;

            cd.card = c;
            cd.targetPos = Vector3.zero + Vector3.right * i * (int)(handPanel.sizeDelta.x / _o.cardsToDeal);
        }

    }

    // Create a UI element for each ability
    void DisplayHabilities()
    {
        abilityTooltipPanel.SetActive(false);
        for (int i = 0; i < player.abilities.Count; i++)
        {
            Ability h = player.abilities[i];

            GameObject go = (GameObject)Instantiate(blankAbility);
            go.transform.SetParent(abilityPanel);
            go.transform.localScale = Vector3.one;
            go.GetComponent<AbilityDisplay>().h = h;

            habilities.Add(go.GetComponent<AbilityDisplay>());
        }
    }

    void DisplayInventory()
    {

    }


    public void EndTurn()
    {
        // End current turn
        if (turns.Peek() == player) DiscardAll();
        turns.Enqueue(turns.Dequeue());

        // Start next turn
        turns.Peek().StartTurn();
        if (turns.Peek() == player) Deal();
    }

    // Register Selected Tokens
    public void SelectedTokenAdd(Collection.TokenTypes token, int value, bool refresh = true)
    {
        selectedTokens[token] += value;

        if (refresh) RefreshHabilities();
    }

    public void SelectedTokenAdd(Collection.TokenValue[] tokenValues)
    {
        for (int i = 0; i < tokenValues.Length; i++) SelectedTokenAdd(tokenValues[i].type, tokenValues[i].value, false);

        RefreshHabilities();
    }

    public void SelectedTokenRemove(Collection.TokenTypes token, int value, bool refresh = true)
    {
        selectedTokens[token] -= value;

        if (selectedTokens[token] < 0)
        {
            Debug.LogWarning("[Manager::SelectedTokenRemove] Token value went negative. <" + token.ToString() + ", " + selectedTokens[token].ToString() + ">");
            selectedTokens[token] = 0;
        }

        if (refresh) RefreshHabilities();
    }

    public void SelectedTokenRemove(Collection.TokenValue[] tokenValues)
    {
        for (int i = 0; i < tokenValues.Length; i++) SelectedTokenRemove(tokenValues[i].type, tokenValues[i].value, false);

        RefreshHabilities();
    }

    // Check if the selected tokens meet any habilities requirements and refresh their appearance
    void RefreshHabilities()
    {
        for (int i = 0; i < habilities.Count; i++)
            habilities[i].Refresh(Collection.EqualTokenCost(selectedTokens,habilities[i].h.tokenCost));
    }

    public void DiscardSelected()
    {
        for (int i = handPanel.childCount - 1; i >= 0; i--)
        {
            CardDisplay cd = handPanel.GetChild(i).GetComponent<CardDisplay>();

            if (!cd.selected) continue;

            SelectedTokenRemove(cd.card.tokenValues);
            discard.Add(cd.card);
            Destroy(cd.gameObject);
        }

        RefreshHabilities();
    }

    // Discard all cards in hand. Reset selected tokens count.
    void DiscardAll()
    {
        for (int i = handPanel.transform.childCount - 1; i >= 0; i--)
        {
            Transform t = handPanel.transform.GetChild(i);
            t.SetParent(null);
            discard.Add(t.GetComponent<CardDisplay>().card);
            Destroy(t.gameObject);
        }

        // Set selected tokens to 0
        foreach (Collection.TokenTypes token in System.Enum.GetValues(typeof(Collection.TokenTypes)))
            selectedTokens[token] = 0;
    }

    public void SetTarget(Enemy e)
    {
        target = e;
    }

    public void EnemyDefeated(Enemy e)
    {
        _enemies.Remove(e);

        if (_enemies.Count == 0) Win(); 
        else SetTarget(_enemies[0]);
    }
    
    public void Win()
    {
        Debug.Log("GAME WON");
        _o.enemyLevel += 1;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void CreateEnemy(int i)
    {
        GameObject go = Instantiate(enemyPrefab);
        go.transform.SetParent(enemiesPanel);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero + _ENEMY_SEPARATION_ * Vector3.right;

        Enemy e = go.GetComponent<Enemy>();
        e.level = _o.enemyLevel;
        e.data = _o.enemies[i];

        // Set all enemy parameters here, else wrong parameters for the first turn.
        e.health = e.data.health + e.level * e.data.healthGrow;
        e.strength = e.data.strength + e.level * e.data.strengthGrow;
        e.energy = e.data.energy + e.level * e.data.energyGrow;
        e.shieldRatio += Enemy._SHIELD_GRWOTH_ * e.level;

        _enemies.Add(e);
    }
}
