using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Component attach to every item in the inventory.
// It sets up the inventory tooltip to show its information.
public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    //[HideInInspector]
    public Item item;
    public ItemTooltip tooltip;
    public bool owned = true;

    public void OnPointerEnter(PointerEventData e)
    {
        tooltip.item = item;
        tooltip.owned = owned;
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        tooltip.gameObject.SetActive(false);
    }
}
