using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField]
    private GameObject ringPrefab;

    private List<GameObject> rings;

    [SerializeField]
    private Transform ringStartPos;
    [SerializeField]
    private float distanceBetweenRings;

    public void SpawnRings(int amount)
    {
        rings = new List<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            GameObject newRing = Instantiate(ringPrefab);
            newRing.transform.position = ringStartPos.position + new Vector3(i * distanceBetweenRings, 0, 0);
            rings.Add(newRing);
        }
    }
    public void RemoveRing()
    {
        GameObject removedRing = rings[rings.Count - 1];
        rings.Remove(removedRing);
        Destroy(removedRing);
    }
}
