using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Populates the information of the item tooltip panel
public class ItemTooltip : MonoBehaviour {

    public Item item;
    public Text title;
    public Text description;
    public Text cost;
    public RectTransform cardContainer;
    public bool owned = true;

    Vector3 displacement = new Vector3(10, -7, 0);

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (item == null)
        {
            Debug.LogWarning("[ItemTooltip::OnEnable] Trting to display an item but the item is null.");
            this.gameObject.SetActive(false);
            return;
        }

        this.transform.position = Input.mousePosition + displacement;

        if (title.text == item.name) return;

        title.text = item.name;
        description.text = item.description;

        if (owned) cost.text = "Drop cost: " + (int)(Overlord._SELL_RATIO_ * item.cost); 
        else cost.text = "Earn benefit: " + (-item.cost);

        // Clear the current cards in the container
        for (int i = cardContainer.childCount - 1; i >= 0; i--)
        {
            Transform t = cardContainer.GetChild(i);

            t.SetParent(null);
            t.localScale = Vector3.zero;
            Destroy(t.gameObject);
        }

        // Display cards
        foreach (Card c in item.cards)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(cardContainer);
            go.transform.localScale = Vector3.one;

            Image img = go.AddComponent<Image>();
            img.preserveAspect = true;
            img.sprite = c.artwork;
        }
    }

    private void Update()
    {
        this.transform.position = Input.mousePosition + displacement;
    }
}
