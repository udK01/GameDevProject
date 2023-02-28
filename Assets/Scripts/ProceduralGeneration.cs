using UnityEngine;
using System.Collections.Generic;

public enum Options { ROAD, SIDEWALK, WATER, REVERSEROAD }

public class ProceduralGeneration : MonoBehaviour
{
    private Options option;
    private Player player;
    private const int ignore = 2;
    private const int laneWidth = 14;
    private const int obstacleSpawnDistance = 10;

    private int i;
    private int ignoreSideWalk = ignore;
    private int ignoreWater = ignore - 1;
    private int ignoreRoad = ignore - 1;
    private int ignoreReverseRoad = ignore - 1;
    private int laneSpeed;
    private int previousLaneSpeed;

    [SerializeField] private Sprite[] carSprites;
    [Header("Game Objects")]
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject reverseRoad;
    [SerializeField] private GameObject sidewalk;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject log;
    [SerializeField] private GameObject turtle;
    [SerializeField] private GameObject star;
    [SerializeField] private GameObject powerUp;
    [Header("Lanes")]
    [SerializeField] private int lanes;
    [Header("Road/Water")]
    [SerializeField] private int minRoadSize;
    [SerializeField] private int maxRoadSize;
    [SerializeField] private int minWaterSize;
    [SerializeField] private int maxWaterSize;
    [Header("Cars")]
    [SerializeField] private int minCars;
    [SerializeField] private int maxCars;
    [SerializeField] private int minCarSpeed;
    [SerializeField] private int maxCarSpeed;
    [Header("Turtles")]
    [SerializeField] private int minTurtles;
    [SerializeField] private int maxTurtles;
    [SerializeField] private int turtleMinSpeed;
    [SerializeField] private int turtleMaxSpeed;
    [Header("Logs")]
    [SerializeField] private int minLogs;
    [SerializeField] private int maxLogs;
    [SerializeField] private int logMinSpeed;
    [SerializeField] private int logMaxSpeed;
    [Header("Loot")]
    [SerializeField] private int lootRadius;
    [SerializeField] private int extraLootSpawnDistance;
    [SerializeField] private int starChance;
    [SerializeField] private int powerUpChance;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        DetectAndSpawnObstacle();
    }

    private void DetectAndSpawnObstacle()
    {
        Vector3 boxPosition = new Vector3(0, player.transform.position.y + obstacleSpawnDistance, 0);
        Vector2 obstacleSize = new Vector2(1, 1);
        Collider2D obstacleExists = Physics2D.OverlapBox(boxPosition, obstacleSize, LayerMask.GetMask("Obstacle"));
        if (!obstacleExists && player.transform.position.y != 0)
        {
            Generate(i, (int)boxPosition.y);
        }
    }

    // S - Start, F - Finish.
    public void Generate(int s, int f)
    {
        for (i = s; i < f; i++)
        {
            option = (Options)Random.Range(0, 4);
            switch (option)
            {
                case Options.ROAD:
                    if (ignoreRoad == 0)
                    {
                        GenerateRoad(i--, 1);
                    }
                    else
                    {
                        DecrementType(ignoreRoad);
                    }
                    break;
                case Options.REVERSEROAD:
                    if (ignoreReverseRoad == 0)
                    {
                        GenerateRoad(i--, 2);
                    }
                    else
                    {
                        DecrementType(ignoreReverseRoad);
                    }
                    break;
                case Options.SIDEWALK:
                    if (ignoreSideWalk == 0)
                    {
                        GenerateSidewalk(i);
                    } else
                    {
                        DecrementType(ignoreSideWalk);
                    }
                    break;
                case Options.WATER:
                    if (ignoreWater == 0)
                    {
                        GenerateWater(i--);
                    }
                    else
                    {
                        DecrementType(ignoreWater);
                    }
                    break;
            }
        }
    }

    private void GenerateRoad(int y, int roadType)
    {
        switch (roadType)
        {
            case 1:
                for (int j = y; j < (y + Random.Range(minRoadSize, maxRoadSize)); j++)
                {
                    GenerateRoadType(j, ignoreRoad, road); // Regular Road.
                }
                break;
            case 2:
                for (int j = y; j < (y + Random.Range(minRoadSize, maxRoadSize)); j++)
                {
                    GenerateRoadType(j, ignoreReverseRoad, reverseRoad); // Reverse Road.
                }
                break;
        }
    }

    private void GenerateWater(int y)
    {
        for (int j = y; j < (y+Random.Range(minWaterSize, maxWaterSize)); j++)
        {
            GameObject waterParent = SpawnObj(water, 0, j);
            if (GetDirection() == Vector2.right)
            {
                RandomSpeed(logMinSpeed, logMaxSpeed);
                GenerateWaterObj(j, Random.Range(minLogs, maxLogs),waterParent, log, GetDirection(), laneSpeed);
            } else
            {
                RandomSpeed(turtleMinSpeed, turtleMaxSpeed);
                GenerateWaterObj(j, Random.Range(minTurtles, maxTurtles),waterParent, turtle, GetDirection(), laneSpeed);
            }
            ignoreWater = ignore - 1;
            i++;
        }
    }

    private void GenerateSidewalk(int y)
    {
        SpawnObj(sidewalk, 0, y);
        ignoreSideWalk = ignore;
    }

    public void GenerateBarrier(int y)
    {
        SpawnObj(barrier, 0, y);
    }

    private void GenerateRoadType(int j, int ignoreType, GameObject roadType)
    {
        GameObject roadParent = SpawnObj(roadType, 0, j);
        RandomSpeed(minCarSpeed, maxCarSpeed);
        GenerateCar(j, Random.Range(minCars, maxCars), roadParent, GetDirection(), laneSpeed);
        ignoreType = ignore - 1;
        i++;
    }

    private void GenerateCar(int y, int n, GameObject parentObj, Vector2 direction, int speed)
    {
        Sprite sprite = carSprites[Random.Range(0, 9)];
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            SpawnObj(parentObj, car, Random.Range(spArray[j] + 1, spArray[j + 1] - 1), y, direction, speed, 1, sprite);
        }
    }

    private void GenerateWaterObj(int y, int n, GameObject parentObj, GameObject obj, Vector2 direction, int speed)
    {
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            SpawnObj(parentObj, obj, Random.Range(spArray[j] + 1, spArray[j + 1] - 1), y,
                direction, speed, 1, obj.GetComponent<SpriteRenderer>().sprite);
        }
    }

    public void GenerateLoot(Vector2 lootCoords, string lootType)
    {
        Vector2 lootPos = new Vector2(lootCoords.x, lootCoords.y + extraLootSpawnDistance);
        Collider2D lootExists = Physics2D.OverlapCircle(lootPos, lootRadius, LayerMask.GetMask(lootType));
        switch (lootType)
        {
            case "Star":
                SpawnLoot(star, starChance, lootExists, lootPos);
                break;
            case "PowerUp":
                SpawnLoot(powerUp, powerUpChance, lootExists, lootPos);
                break;
        }

    }

    // SpawnLoot.
    private void SpawnLoot(GameObject obj, int chance, Collider2D lootExists, Vector2 lootPos)
    {
        if (Random.Range(0, chance) == 1 && lootExists == null)
        {
            SpawnObj(obj, Random.Range(-laneWidth / 2, laneWidth / 2), (int)lootPos.y);
        }
    }

    // Spawn Stationary Objects.
    private GameObject SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
        return obj;
    }

    // Spawn Moving Objects.
    private void SpawnObj(GameObject parentObj, GameObject obj, int width, int height, Vector2 direction, int speed, double size, Sprite sprite)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);

        Obstacle obstacle = obj.gameObject.GetComponent<Obstacle>();
        SpriteRenderer spriteRenderer = obj.gameObject.GetComponent<SpriteRenderer>();

        obstacle.SetDirection(direction);
        obstacle.SetSpeed(speed);
        obstacle.SetSize(size);

        spriteRenderer.sprite = sprite;
        if (direction == Vector2.left)
        {
            spriteRenderer.flipX = true;
        }

        obj.transform.parent = parentObj.transform;
    }

    private void DecrementType(int ignoreType)
    {
        i--;
        ignoreType--;
    }

    private void RandomSpeed(int x, int y)
    {
        laneSpeed = Random.Range(x, y);
        if (laneSpeed == previousLaneSpeed)
        {
            RandomSpeed(x, y);
        }
        previousLaneSpeed = laneSpeed;
    }

    public void ResetMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateBarrier(-2);
        GenerateSidewalk(0);
        Generate((int)player.transform.position.y + 1, lanes);
    }

    private Vector2 GetDirection()
    {
        Vector2[] directions = { Vector2.left, Vector2.right };
        return directions[Random.Range(0, 2)];
    }

    private int[] CalculateSpawnPoints(int n)
    {
        int[] spArray = new int[n + 1]; //spawn points
        spArray[0] = 1;
        for (int k = 1; k < spArray.Length; k++)
        {
            spArray[k] = k * (laneWidth / n);
        }
        return spArray;
    }
}
