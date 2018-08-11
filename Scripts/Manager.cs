using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public Player player;

    [Header("Habilities UI Management")]
    public Transform habilityPanel;
    public GameObject costToken;
    public GameObject blankHability;
    public Color swordColor;
    public Color shieldColor;

    [Header("Card Management")]
    // List of playable cards owned by the player
    public List<Card> collection;

    // How many cards should be dealt wech turn
    public int cardsToDeal = 5;

    public RectTransform handPanel;
    public GameObject cardUI;

    // Cards currently on the draft pile
    [SerializeField]
    List<Card> pile;

    // Cards currently in the discard pile
    [SerializeField]
    List<Card> discard;


	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        discard = new List<Card>();
        pile = Collection.Shuffle(collection);

        Deal();
        DisplayHabilities();
        DisplayInventory();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // TODO: An animation when you reshuffle the cards
    // TODO: Used cards go to a discard pile instead of banishing.
    void Deal()
    {
        // Erase previous hand if there is anythin on hand
        for (int i = handPanel.transform.childCount - 1; i >= 0; i--)
        {
            Transform t = handPanel.transform.GetChild(i);
            t.SetParent(null);
            discard.Add(t.GetComponent<CardDisplay>().card);
            Destroy(t.gameObject);
        }

        // Deal the required number of cards to hand
        for (int i = 0; i < cardsToDeal; i++)
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
            cd.targetPos = Vector3.zero + Vector3.right * i * (int)(handPanel.sizeDelta.x / cardsToDeal);
        }

    }

    // Create a UI element for each hability
    void DisplayHabilities()
    {
        for (int i = 0; i < player.habilities.Count; i++)
        {
            Hability h = player.habilities[i];

            GameObject go = (GameObject)Instantiate(blankHability);
            go.transform.SetParent(habilityPanel);
            go.transform.localScale = Vector3.one;
            go.transform.Find("Hability Name").GetComponent<Text>().text = h.name;

            Transform t = go.transform.Find("Cost Layout");
            PlaceTokens(swordColor, h.swordCost, t);
            PlaceTokens(shieldColor, h.shieldCost, t);
        }
    }

    void DisplayInventory()
    {

    }

    // Places the cost tokens for an hability with the appropiate color
    void PlaceTokens(Color color, int numTokens, Transform parent)
    {
        for (int j = 0; j < numTokens; j++)
        {
            GameObject token = Instantiate(costToken);
            token.transform.SetParent(parent);
            token.transform.localScale = Vector3.one;
            token.GetComponent<Image>().color = color;
        }
    }

    public void EndTurn()
    {
        Deal();
    }

}
