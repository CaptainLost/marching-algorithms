using UnityEngine;

public interface IMarchingCubes
{
    bool IsValidCell(Vector3Int cell);
    void SetWeight(Vector3Int cell, float weight);
    float GetGlobalWeight(Vector3Int cell);
    bool TryGetWeight(Vector3Int cell, out float weight);
}
