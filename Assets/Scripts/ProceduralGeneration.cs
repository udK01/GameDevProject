using UnityEngine;

public enum Options { ROAD, SIDEWALK, WATER }

public class ProceduralGeneration : MonoBehaviour
{
    private Options option;
    private int i;

    private const int ignore = 2;
    private const int laneWidth = 14;

    private Player player;
    private int ignoreSideWalk = ignore;
    private int ignoreWater = ignore - 1;
    private int ignoreRoad = ignore - 1;
    [SerializeField] Sprite[] carSprites;

    [SerializeField] GameObject road;
    [SerializeField] GameObject sidewalk;
    [SerializeField] GameObject water;
    [SerializeField] GameObject barrier;
    [SerializeField] GameObject car;
    [SerializeField] GameObject log;
    [SerializeField] GameObject turtle;
    [SerializeField] GameObject star;
    [SerializeField] GameObject powerUp;

    [SerializeField] int lanes;
    [SerializeField] int minRoadSize;
    [SerializeField] int maxRoadSize;
    [SerializeField] int minWaterSize;
    [SerializeField] int maxWaterSize;
    [SerializeField] int lootRadius;
    [SerializeField] int extraLootSpawnDistance;
    [SerializeField] int starChance;
    [SerializeField] int powerUpChance;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Generate();
    }

    private void Generate()
    {
        InitializeMap();
        for (i = 1; i < lanes; i++)
        {
            option = (Options)Random.Range(0, 3);
            switch (option)
            {
                case Options.ROAD:
                    if (ignoreRoad == 0)
                    {
                        GenerateRoad(i);
                    }
                    else
                    {
                        i--;
                        ignoreRoad--;
                    }
                    break;
                case Options.SIDEWALK:
                    if (ignoreSideWalk == 0)
                    {
                        GenerateSidewalk(i);
                    } else
                    {
                        i--;
                        ignoreSideWalk--;
                    }
                    break;
                case Options.WATER:
                    if (ignoreWater == 0)
                    {
                        GenerateWater(i);
                    }
                    else
                    {
                        i--;
                        ignoreWater--;
                    }
                    break;
            }
        }
        GenerateBarrier(i+1);
    }

    private void InitializeMap()
    {
        GenerateBarrier(-2);
        GenerateSidewalk(0);
    }

    private void GenerateRoad(int y)
    {
        for (int j = y; j < (y+Random.Range(minRoadSize, maxRoadSize)); j++)
        {
            SpawnObj(road, 0, j);
            GenerateCar(j, Random.Range(1,5), GetDirection(), RandomSpeed(1,4));
            ignoreRoad = ignore - 1;
            i++;
        }
        i--;
    }

    private int RandomSpeed(int x, int y)
    {
        return Random.Range(x,y);
    }

    private Vector2 GetDirection()
    {
        Vector2[] directions = { Vector2.left, Vector2.right };
        return directions[Random.Range(0, 2)];
    }

    private void GenerateCar(int y, int n, Vector2 direction, float speed)
    {
        Sprite sprite = carSprites[Random.Range(0, 9)];
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        { 
            SpawnObj(car, Random.Range(spArray[j]+1,spArray[j+1]-1), y, direction, speed, 1, sprite);
        }
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

    private void GenerateWater(int y)
    {
        for (int j = y; j < (y+Random.Range(minWaterSize, maxWaterSize)); j++)
        {
            SpawnObj(water, 0, j);
            if (GetDirection() == Vector2.right)
            {
                GenerateLog(j, Random.Range(1, 5), GetDirection(), RandomSpeed(1, 5));
            } else
            {
                GenerateTurtle(j, Random.Range(4, 8), GetDirection(), RandomSpeed(1, 3));
            }
            
            ignoreWater = ignore - 1;
            i++;
        }
        i--;
    }

    private void GenerateLog(int y, int n, Vector2 direction, float speed)
    {
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            SpawnObj(log, Random.Range(spArray[j]+1, spArray[j + 1]-1), y, 
                direction, speed, 1, log.gameObject.GetComponent<SpriteRenderer>().sprite);
        }
    }

    private void GenerateTurtle(int y, int n, Vector2 direction, float speed)
    {
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            SpawnObj(turtle, Random.Range(spArray[j] + 1, spArray[j + 1]), y,
                direction, speed, 1, turtle.gameObject.GetComponent<SpriteRenderer>().sprite);
        }
    }

    private void GenerateSidewalk(int y)
    {
        SpawnObj(sidewalk, 0, y);
        ignoreSideWalk = ignore;
    }

    private void GenerateBarrier(int y)
    {
        SpawnObj(barrier, 0, y);
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
    private void SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }

    // Spawn Moving Objects.
    private void SpawnObj(GameObject obj, int width, int height, Vector2 direction, float speed, double size, Sprite sprite)
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

        obj.transform.parent = this.transform;
    }
}
