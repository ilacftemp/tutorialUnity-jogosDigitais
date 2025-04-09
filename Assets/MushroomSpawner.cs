using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public Transform collectiblesParent;

    public int maxMushrooms = 5;
    private Queue<GameObject> mushroomQueue = new Queue<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);

            Vector2 randomPos = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            GameObject mushroom = Instantiate(mushroomPrefab, randomPos, Quaternion.identity, collectiblesParent);

            mushroomQueue.Enqueue(mushroom);

            if (mushroomQueue.Count > maxMushrooms)
            {
                GameObject oldest = mushroomQueue.Dequeue();
                Destroy(oldest);
            }
        }
    }
}