using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour {

    public Text description;
    public Image[] itemImage;


    System.Random rng = new System.Random(System.DateTime.Now.Millisecond+7);
    Overlord _o;
    Item[] items;

    private void Awake()
    {
        _o = Overlord._instance;
        items = new Item[itemImage.Length];
    }

    // Use this for initialization
    void OnEnable ()
    {
        for (int i = 0; i < itemImage.Length; i++)
        {
            int r = rng.Next(_o.currentEvent.rewards.Length);
            items[i] = _o.currentEvent.rewards[r];

            itemImage[i].sprite = items[i].artwork;
            itemImage[i].GetComponent<ItemDisplay>().item = items[i];
        }

        if (_o.inventory.Count == _o.inventorySlots) description.text = "Your inventory is full and this drives you insane. Choose an item to drop.";
        else description.text = "Choose your reward.";
    }

    public void SelectItem(int index)
    {
        _o.InventoryAdd(items[index]);
        Manager m =  FindObjectOfType<Manager>();
        if (m != null) m.DisplayInventory();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}
