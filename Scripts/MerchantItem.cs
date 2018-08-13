using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MerchantItem : ItemDisplay, IPointerClickHandler {
    

    public void OnPointerClick(PointerEventData e)
    {
        if (owned) FindObjectOfType<Merchant>().Sell(this.item);
        else FindObjectOfType<Merchant>().Buy(this.item);
    }
}
