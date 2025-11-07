using System;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    // public GameObject meteorPrefab;     // Prefab thiên thạch (chung)
    //public Sprite[] meteorSprites;      // Danh sách sprite thiên thạch
    public GameObject[] lmeteorPrefabs;
    public float spawnRate = 2f;
    public float minY = -4f, maxY = 4f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnRate);
    }

    void SpawnMeteor()
    {
        // Tọa độ spawn (bên phải màn hình)
        float randomY = UnityEngine.Random.Range(minY, maxY);
        float spawnX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1f;
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        // Tạo meteor
        // GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // Gán sprite ngẫu nhiên nếu có
        if (lmeteorPrefabs.Length > 0)
        {
            //Sprite randomSprite = meteorSprites[UnityEngine.Random.Range(0, meteorSprites.Length)];
            //SpriteRenderer sr = meteor.GetComponent<SpriteRenderer>();
            //if (sr != null)
            //{
            //    sr.sprite = randomSprite;
            //}
            int index = UnityEngine.Random.Range(0, lmeteorPrefabs.Length);
            Instantiate(lmeteorPrefabs[index], spawnPos, Quaternion.identity);
        }
    }
}