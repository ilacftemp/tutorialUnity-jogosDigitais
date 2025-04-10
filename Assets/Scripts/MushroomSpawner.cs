using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct TriangleArea
{
    public Vector2 A;
    public Vector2 B;
    public Vector2 C;

    public bool Contains(Vector2 P)
    {
        float Area(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return Mathf.Abs((p1.x * (p2.y - p3.y) +
                              p2.x * (p3.y - p1.y) +
                              p3.x * (p1.y - p2.y)) / 2f);
        }

        float areaABC = Area(A, B, C);
        float areaPBC = Area(P, B, C);
        float areaAPC = Area(A, P, C);
        float areaABP = Area(A, B, P);

        return Mathf.Abs(areaABC - (areaPBC + areaAPC + areaABP)) < 0.0001f;
    }
}

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public Transform collectiblesParent;
    public int maxMushrooms = 5;

    private Vector2 minPosition;
    private Vector2 maxPosition;
    private Queue<GameObject> mushroomQueue = new Queue<GameObject>();
    private List<TriangleArea> forbiddenTriangles;

    void Awake()
    {
        CalculateBounds();
        InitializeForbiddenAreas();
    }

    void CalculateBounds()
    {
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        minPosition = new Vector2(-cameraWidth / 2f, -cameraHeight / 2f);
        maxPosition = new Vector2(cameraWidth / 2f, cameraHeight / 2f);
    }

    void InitializeForbiddenAreas()
    {
        forbiddenTriangles = new List<TriangleArea>
        {
            new TriangleArea { A = new Vector2(-9f, 5.2f), B = new Vector2(-9f, 1.2f), C = new Vector2(-3f, 5.3f) },
            new TriangleArea { A = new Vector2(-5f, 2.5f), B = new Vector2(-7f, 2.5f), C = new Vector2(-4f, 5.3f) },
            new TriangleArea { A = new Vector2(9f, 5.2f),  B = new Vector2(9f, 1f),   C = new Vector2(3f, 5.3f) },
            new TriangleArea { A = new Vector2(9f, -5.2f), B = new Vector2(9f, 1f),   C = new Vector2(2f, -5.3f) },
            new TriangleArea { A = new Vector2(-9f, -5.2f), B = new Vector2(-9f, -1.2f), C = new Vector2(-2f, -5.3f) },
            new TriangleArea { A = new Vector2(-8f, 0f), B = new Vector2(-9f, -1.2f), C = new Vector2(-9f, 1f) },
            new TriangleArea { A = new Vector2(9f, 1f), B = new Vector2(7.6f, 0f), C = new Vector2(7.6f, 2f) }
        };
    }

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

            Vector2 randomPos;
            bool isInForbiddenArea;

            do
            {
                randomPos = new Vector2(
                    Random.Range(minPosition.x, maxPosition.x),
                    Random.Range(minPosition.y, maxPosition.y)
                );

                isInForbiddenArea = false;
                foreach (var triangle in forbiddenTriangles)
                {
                    if (triangle.Contains(randomPos))
                    {
                        isInForbiddenArea = true;
                        break;
                    }
                }

            } while (isInForbiddenArea);

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