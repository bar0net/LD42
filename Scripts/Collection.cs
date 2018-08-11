using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collection {

    // Shuffle cards
    public static List<Card> Shuffle(List<Card> collection)
    {
        System.Random rng = new System.Random();

        Card c;

        int n = collection.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(rng.NextDouble() * (n - i));
            c = collection[r];
            collection[r] = collection[i];
            collection[i] = c;
        }

        return collection;
    }
}
