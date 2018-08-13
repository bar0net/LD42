using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Map : MonoBehaviour {

    public int numEventsRound = 3;

    public Location[] locations;

    public Overlord.EventTypes[] events;

    public ItemDisplay[] itemsUI;
    public ItemTooltip itemTooltip;

    public Text hpText;
    public Text insanityText;

    Overlord _o;
    System.Random rng = new System.Random(System.DateTime.Now.Millisecond + 3);

    private void Start()
    {
        _o = Overlord._instance;

        if (_o.eventsCompleted % 10 == 0 && _o.eventsCompleted != 0)
        {
            locations[rng.Next(locations.Length)].SetEvent( _o.CreateEvent(_o.bossFights) );
        }
        else
        {
            int c = 0;
            for (int i = 0; i < numEventsRound; i++)
            {
                c = (c + rng.Next(locations.Length)) % locations.Length;

                for (int j = 0; j < locations.Length; j++)
                {
                    if (locations[c].locationEvent != null) continue;

                    Overlord.EventTypes et = events[rng.Next(events.Length)];
                    locations[c].SetEvent( _o.GetRandomEvent(et) );
                    break;
                }
            }
        }

        hpText.text = "HP: " + PlayerPrefs.GetInt("player_health", 100).ToString();
        insanityText.text = "Insanity: " + _o.enemyLevel;

        for (int i = 0; i < itemsUI.Length; i++)
        {
            if (i < _o.inventory.Count)
            {
                itemsUI[i].enabled = true;
                itemsUI[i].item = _o.inventory[i];
                itemsUI[i].tooltip = itemTooltip;
                Image img = itemsUI[i].gameObject.GetComponent<Image>();
                img.sprite = _o.inventory[i].artwork;
                img.color = Color.white;
            }
            else
            {
                itemsUI[i].enabled = false;
            }
        }
    }
}
