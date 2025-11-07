using System;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public float spawnRate = 2f;
    public float minY = -4f, maxY = 4f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnStar), 1f, spawnRate);
    }

    void SpawnStar()
    {
        float randomY = UnityEngine.Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(-Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - 1f, randomY, 0);
        Instantiate(starPrefab, spawnPos, Quaternion.identity);
    }
}
