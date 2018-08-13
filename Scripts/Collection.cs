using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collection {
    // Types of tokens
    public enum TokenTypes { SWORD, SHIELD, ORB }

    // Store tokens cost / value
    [System.Serializable]
    public struct TokenValue
    {
        public Collection.TokenTypes type;
        public int value;
    }

    // Store token colors
    [System.Serializable]
    public struct TokenColor
    {
        public Collection.TokenTypes type;
        public Color value;
    }

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

    public static bool EqualTokenCost(Dictionary<TokenTypes,int> dict, TokenValue[] list)
    {
        List<TokenTypes> types = new List<TokenTypes>();
        foreach (Collection.TokenTypes token in System.Enum.GetValues(typeof(Collection.TokenTypes))) types.Add(token);

        for (int i = 0; i < list.Length; i++)
        {
            if (!dict.ContainsKey(list[i].type)) return false;
            if (dict[list[i].type] != list[i].value) return false;

            types.Remove(list[i].type);
        }

        for (int i = 0; i < types.Count; i++)
        {
            if (!dict.ContainsKey(types[i])) continue;
            if (dict[types[i]] > 0) return false;
        }

        return true;
    }

}
