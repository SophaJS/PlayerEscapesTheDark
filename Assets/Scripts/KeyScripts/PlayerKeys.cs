using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    private HashSet<string> keys = new HashSet<string>();

    // Event that notifies when a key is added
    public static event Action<string> OnKeyCollected;

    public void AddKey(string keyID)
    {
        if (!keys.Contains(keyID))
        {
            keys.Add(keyID);
            Debug.Log("Picked up key: " + keyID);
            OnKeyCollected?.Invoke(keyID); // notify listeners (like doors)
        }
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }
}
