using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorTilemap : MonoBehaviour
{
    public string requiredKey = "default";
    private Tilemap tilemap;
    private BoundsInt doorArea;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        doorArea = tilemap.cellBounds; // capture the region of tiles this door covers
    }

    private void OnEnable()
    {
        PlayerKeys.OnKeyCollected += HandleKeyCollected;
    }

    private void OnDisable()
    {
        PlayerKeys.OnKeyCollected -= HandleKeyCollected;
    }

    private void HandleKeyCollected(string keyID)
    {
        if (keyID == requiredKey)
        {
            Debug.Log("Tilemap door unlocked!");
            ClearDoorTiles();
        }
    }

    private void ClearDoorTiles()
    {
        // Loops through all tiles in the door’s bounds and removes them
        foreach (var pos in doorArea.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tilemap.SetTile(pos, null);
            }
        }
    }
}
