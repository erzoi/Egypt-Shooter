using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnRadius = 45f;
    [SerializeField] private float maxSpawnRadius = 90f;
    [SerializeField] private int maxObjectCount = 5;
    [SerializeField] private bool changeSpawnInterval = false;

    [Header("Timing")]
    [SerializeField] private float baseSpawnInterval = 15f;

    private float timer;
    private int currentObjectCount;

    void Update()
    {
        timer += Time.deltaTime;

        float currentInterval = baseSpawnInterval;

        if (changeSpawnInterval) currentInterval = GetCurrentSpawnInterval();

        if (timer >= currentInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    float GetCurrentSpawnInterval()
    {
        int stage = Mathf.FloorToInt(GameTimeManager.GameTime / 15f);

        float interval = baseSpawnInterval - stage * 0.5f;

        return Mathf.Clamp(interval, 1f, baseSpawnInterval);
    }

    void SpawnObject()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = GetRandomPointInDonut(
                playerTransform.position,
                minSpawnRadius,
                maxSpawnRadius
            );

            if (!TryGetNavMeshPoint(randomPoint, out Vector3 navPoint))
                continue;

            if (IsVisibleToCamera(navPoint))
                continue;

            if (currentObjectCount < maxObjectCount)
            {
                GameObject obj = Instantiate(objectPrefab, navPoint, Quaternion.identity);

                var spawnable = obj.GetComponent<ISpawnable>();
                spawnable?.Initialize(this);

                currentObjectCount++;
            }

            return;
        }
    }

    Vector3 GetRandomPointInDonut(Vector3 center, float minRadius, float maxRadius)
    {
        float randomDistance = Random.Range(minRadius, maxRadius);
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 offset = new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;

        return center + offset;
    }

    bool TryGetNavMeshPoint(Vector3 origin, out Vector3 result)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(origin, out hit, 2f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    bool IsVisibleToCamera(Vector3 position)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);

        return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1;
    }

    public void OnObjectDestroy()
    {
        currentObjectCount--;
    }
}
