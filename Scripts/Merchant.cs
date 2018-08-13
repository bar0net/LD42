using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Merchant : MonoBehaviour {

    public GameObject interactableItem;

    [Header("Panels")]
    public Transform sellingLayout;
    public Transform buyingLayout;
    public GameObject confirmationPanel;
    public ItemTooltip tooltip;


    Overlord _o;

    bool selling = true;
    [SerializeField]
    Item operationItem = null;

	// Use this for initialization
	void Start ()
    {
        _o = Overlord._instance;
        if (_o.currentEvent == null) _o.currentEvent = _o.GetRandomEvent(Overlord.EventTypes.MERCHANT);

        confirmationPanel.SetActive(false);

        RefreshInventory();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Sell(Item item)
    {
        selling = true;
        operationItem = item;

        if (confirmationPanel != null) confirmationPanel.SetActive(true);
    }

    public void Buy(Item item)
    {
        if (_o.inventorySlots == _o.inventory.Count) return;

        selling = false;
        operationItem = item;

        if (confirmationPanel != null) confirmationPanel.SetActive(true);
    }

    public void ConfirmOperation()
    {
        if (selling)
        {
            _o.InventoryRemove(operationItem);
            RefreshLayout(sellingLayout, _o.inventorySlots, true, _o.inventory.ToArray());
        }
        else
        {
            _o.InventoryAdd(operationItem);
            for (int i = 0; i < _o.currentEvent.rewards.Length; i++)
            {
                if (_o.currentEvent.rewards[i] != operationItem) continue;
                _o.currentEvent.rewards[i] = null;
                break;
            }
            RefreshInventory();
        }

        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void CancelOperation()
    {
        operationItem = null;

        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void RefreshInventory()
    {
        RefreshLayout(sellingLayout, _o.inventorySlots, true, _o.inventory.ToArray());
        RefreshLayout(buyingLayout, _o.currentEvent.rewards.Length, false, _o.currentEvent.rewards);
    }

    void ClearLayout(Transform layout)
    {
        for (int i = layout.childCount - 1; i >= 0; i--)
        {
            Transform t = layout.GetChild(i);
            t.SetParent(null);
            t.localScale = Vector3.zero;
            Destroy(t.gameObject);
        }

    }

    void RefreshLayout(Transform layout, int slots, bool owned, Item[] list)
    {
        ClearLayout(layout);

        for (int i = 0; i < slots; i++)
        {
            GameObject go = Instantiate(interactableItem);
            go.transform.SetParent(layout);
            go.transform.localScale = Vector3.one;

            if (i < list.Length && list[i] != null)
            {
                go.GetComponent<ItemDisplay>().item = list[i];
                go.GetComponent<ItemDisplay>().tooltip = tooltip;
                go.GetComponent<ItemDisplay>().owned = owned;
                go.GetComponent<Image>().sprite = list[i].artwork;
                go.GetComponent<Button>().interactable = (owned || _o.inventory.Count < _o.inventorySlots);
            }
            else
            {
                go.GetComponent<Button>().interactable = false;
                go.GetComponent<ItemDisplay>().enabled = false;
            }

        }
    }

    public void LoadScene()
    {
        _o.eventsCompleted += 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}
