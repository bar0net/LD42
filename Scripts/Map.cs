using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Map : MonoBehaviour {

    public int numEventsRound = 3;

    public Location[] locations;

    public Overlord.EventTypes[] events;

    Overlord _o;
    System.Random rng = new System.Random(System.DateTime.Now.Millisecond + 3);

    private void Start()
    {
        _o = FindObjectOfType<Overlord>();

        if (_o.enemyLevel % 10 == 0)
        {
            locations[rng.Next(locations.Length)].locationEvent = Overlord.EventTypes.BOSS_FIGHT;
        }
        else
        {
            int c = 0;
            for (int i = 0; i < numEventsRound; i++)
            {
                c = (c + rng.Next(locations.Length)) % locations.Length;

                for (int j = 0; j < locations.Length; j++)
                {
                    if (locations[c].locationEvent != Overlord.EventTypes.NONE) continue;

                    locations[c].SetEvent(events[rng.Next(events.Length)]);
                    break;
                }
            }
        }

    }
}
