using UnityEngine;

public enum Options { ROAD, WATER, REVERSEROAD, LAVAROAD }

public class ProceduralGeneration : MonoBehaviour
{
    public static ProceduralGeneration Instance { get; private set; }
    private Options option;
    private const int IGNORE = 3;
    private const int LANE_WIDTH = 14;
    private const int OBSTACLE_SPAWN_DISTANCE = 10;

    public int i { get; private set; }
    private int ignoreWater = IGNORE;
    private int ignoreRoad = IGNORE;
    private int ignoreReverseRoad = IGNORE;
    private int ignoreLavaRoad = IGNORE;
    private int laneSpeed;
    private int previousLaneSpeed;

    [SerializeField] private Sprite[] carSprites;
    [SerializeField] private Sprite[] tractorSprites;
    [Header("Game Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject reverseRoad;
    [SerializeField] private GameObject lavaRoad;
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
        Instance = this;
    }

    private void Update()
    {
        DetectAndSpawnObstacle();
    }

    /// <summary>
    /// Detects obstacleSpawnDistance (10) rows ahead the player if the map continues.
    /// If not, it generates a new part for the map.
    /// </summary>
    private void DetectAndSpawnObstacle()
    {
        Vector2 boxPosition = new Vector3(0, Player.Instance.transform.position.y + OBSTACLE_SPAWN_DISTANCE);
        Vector2 obstacleSize = new Vector2(1, 1);
        Collider2D obstacleExists = Physics2D.OverlapBox(boxPosition, obstacleSize, LayerMask.GetMask("Obstacle"));
        if (!obstacleExists && Player.Instance.transform.position.y != 0)
        {
            Generate(i, (int)boxPosition.y);
        }
    }

    /// <summary>
    /// Selects a new obstacle from Options (Road, ReverseRoad, Water)
    /// and generates it between the starting and ending coordinates.
    /// 
    /// If the selected obstacle is currently being ignored, decrement by one row
    /// and run until an available option is selected.
    /// </summary>
    /// <param name="s"> Generation Starting Point </param>
    /// <param name="f"> Generation End Point </param>
    public void Generate(int s, int e)
    {
        for (i = s; i < e; i++)
        {
            option = (Options)Random.Range(0, 4);
            switch (option)
            {
                case Options.ROAD:
                    if (ignoreRoad-- > 0) { i--; } else { GenerateRoad(i--, 1); }
                    break;
                case Options.REVERSEROAD:
                    if (ignoreReverseRoad-- > 0) { i--; } else { GenerateRoad(i--, 2); }
                    break;
                case Options.LAVAROAD:
                    if (ignoreLavaRoad-- > 0) { i--; } else { GenerateRoad(i--, 3); }
                    break;
                case Options.WATER:
                    if (ignoreWater-- > 0) { i--; } else { GenerateWater(i--); }
                    break;
            }
        }
    }

    /// <summary>
    /// Generates a random amount of regular or reverse Road based 
    /// on the type given with a random road size.
    /// 
    /// Adds a Sidewalk after the water is done generating.
    /// </summary>
    /// <param name="y"> Road Starting Point </param>
    /// <param name="roadType"> Road Type (Reverse / Regular Road) </param>
    private void GenerateRoad(int y, int roadType)
    {
        switch (roadType)
        {
            case 1:
                for (int j = y; j < (y + Random.Range(minRoadSize, maxRoadSize)); j++)
                {
                    GenerateRoadType(j, ignoreRoad, road, carSprites); // Regular Road.
                }
                break;
            case 2:
                for (int j = y; j < (y + Random.Range(minRoadSize, maxRoadSize)); j++)
                {
                    GenerateRoadType(j, ignoreReverseRoad, reverseRoad, tractorSprites); // Reverse Road.
                }
                break;
            case 3:
                for (int j = y; j < (y + Random.Range(minRoadSize, maxRoadSize)); j++)
                {
                    GenerateRoadType(j, ignoreLavaRoad, lavaRoad, carSprites); // Reverse Road.
                }
                break;
        }
        GenerateSidewalk(i+1);
    }

    /// <summary>
    /// Generates a random amount of Water rows with a randomly 
    /// chosen amount of objects on top and parents them to it.
    /// 
    /// Reset ignore variable and increments global counter.
    /// Adds a Sidewalk after the water is done generating.
    /// </summary>
    /// <param name="y"> Water Starting Point </param>
    private void GenerateWater(int y)
    {
        for (int j = y; j < (y+Random.Range(minWaterSize, maxWaterSize)); j++)
        {
            GameObject waterParent = SpawnObj(water, 0, j);
            if (GetDirection() == Vector2.right)
            {
                RandomSpeed(logMinSpeed, logMaxSpeed);
                GenerateWaterObj(j, Random.Range(minLogs, maxLogs), waterParent, log, GetDirection(), laneSpeed);
            } else
            {
                RandomSpeed(turtleMinSpeed, turtleMaxSpeed);
                GenerateWaterObj(j, Random.Range(minTurtles, maxTurtles), waterParent, turtle, GetDirection(), laneSpeed);
            }
            ignoreWater = IGNORE;
            i++;
        }
        GenerateSidewalk(i+1);
    }

    /// <summary>
    /// Generates a sidewalk at the specified locaition (y) and
    /// increments the global counter (i).
    /// </summary>
    /// <param name="y"> Sidewalk Starting Point </param>
    private void GenerateSidewalk(int y)
    {
        SpawnObj(sidewalk, 0, y);
        i++;
    }

    /// <summary>
    /// Generates a sidewalk at the specified location (y).
    /// </summary>
    /// <param name="y"> Barrier Starting Point </param>
    public void GenerateBarrier(int y)
    {
        SpawnObj(barrier, 0, y);
    }

    /// <summary>
    /// Creates a road object, selects a car speed, generates 
    /// cars/tractors on top and resets the ignore.
    /// </summary>
    /// <param name="j"> Object Y Coordinate </param>
    /// <param name="ignoreType"> Reset Ignore </param>
    /// <param name="roadType"> Road Type </param>
    /// <param name="sprites"> Car/Tractor Sprite Array </param>
    private void GenerateRoadType(int j, int ignoreType, GameObject roadType, Sprite[] sprites)
    {
        GameObject roadParent = SpawnObj(roadType, 0, j);
        RandomSpeed(minCarSpeed, maxCarSpeed);
        GenerateRoadObj(j, Random.Range(minCars, maxCars), roadParent, GetDirection(), laneSpeed, sprites);
        ignoreType = IGNORE;
        i++;
    }

    /// <summary>
    /// Chooses a Car/Tractor sprite from the given array.
    /// Calculates the spawn coordinates based on the amount of objects,
    /// and spawns the objects on top of the road.
    /// </summary>
    /// <param name="y"> Y Coordinate </param>
    /// <param name="n"> Amount of Cars </param>
    /// <param name="parentObj"> Parent Object </param>
    /// <param name="direction"> Object's Direction (left/right) </param>
    /// <param name="speed"> Object's Speed </param>
    /// <param name="sprites"> Tractor/Car Sprite Array </param>
    private void GenerateRoadObj(int y, int n, GameObject parentObj, Vector2 direction, int speed, Sprite[] sprites)
    {
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            Sprite sprite = sprites[Random.Range(0, sprites.Length)];
            SpawnObj(parentObj, car, Random.Range(spArray[j] + 1, spArray[j + 1] - 1), y, direction, speed, 1, sprite);
        }
    }

    /// <summary>
    /// Calculates the spawn coordinates based on the amount of objects,
    /// and spawns the objects on top of the water.
    /// </summary>
    /// <param name="y"> Y Coordinate </param>
    /// <param name="n"> Amount Of Cars </param>
    /// <param name="parentObj"> Parent Object </param>
    /// <param name="obj"> Obj To Be Spawned </param>
    /// <param name="direction"> Object's Direction (left/right) </param>
    /// <param name="speed"> Object's Speed </param>
    private void GenerateWaterObj(int y, int n, GameObject parentObj, GameObject obj, Vector2 direction, int speed)
    {
        int[] spArray = CalculateSpawnPoints(n);
        for (int j = 0; j < n; j++)
        {
            SpawnObj(parentObj, obj, Random.Range(spArray[j] + 1, spArray[j + 1] - 1), y,
                direction, speed, 1, obj.GetComponent<SpriteRenderer>().sprite);
        }
    }

    /// <summary>
    /// Calculates the loot coordinates, checks if a loot exists within a specified radius around it
    /// and attempts to spawn a Star or PowerUp based on Loot Type.
    /// </summary>
    /// <param name="lootCoords"> Loot Coordinates </param>
    /// <param name="lootType"> Type Of Loot (Star/PowerUp) </param>
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

    /// <summary>
    /// Get a random integer between 1 and (chance-1), and spawns a PowerUp/Star
    /// if there isn't one spawned nearby and they rolled 1.
    /// </summary>
    /// <param name="obj"> Object To Spawn </param>
    /// <param name="chance"> Object Spawn Chance </param>
    /// <param name="lootExists"> Nearby Loot Exists </param>
    /// <param name="lootPos"> Position To Spawn At </param>
    private void SpawnLoot(GameObject obj, int chance, Collider2D lootExists, Vector2 lootPos)
    {
        if (Random.Range(0, chance) == 1 && lootExists == null)
        {
            SpawnObj(obj, Random.Range(-LANE_WIDTH / 2, LANE_WIDTH / 2), (int)lootPos.y);
        }
    }

    /// <summary>
    /// Instantiates a new stationary object at the coordinates 
    /// given and parents it to the existing object.
    /// </summary>
    /// <param name="obj"> Object To Spawn </param>
    /// <param name="width">  Object Width </param>
    /// <param name="height">  Object Height </param>
    /// <returns> Created Object </returns>
    private GameObject SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
        return obj;
    }

    /// <summary>
    /// Instantiates a new object at the specified coordinates, 
    /// gives it the selected sprite and initializes its values (direction, speed, size).
    /// If it's moving left, flip sprite to match it.
    /// </summary>
    /// <param name="parentObj"> Parent Object </param>
    /// <param name="obj"> Object To Spawn </param>
    /// <param name="width"> Object Width </param>
    /// <param name="height"> Object Height </param>
    /// <param name="direction"> Object Direction (Left/Right) </param>
    /// <param name="speed"> Object Speed </param>
    /// <param name="size"> Object Size (Needed For Looping) </param>
    /// <param name="sprite"> Object Sprite </param>
    private void SpawnObj(GameObject parentObj, GameObject obj, int width, int height, Vector2 direction, int speed, double size, Sprite sprite)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);

        Obstacle obstacle = obj.gameObject.GetComponent<Obstacle>();
        SpriteRenderer spriteRenderer = obj.gameObject.GetComponent<SpriteRenderer>();

        obstacle.direction = direction;
        obstacle.speed = speed;
        obstacle.size = size;

        spriteRenderer.sprite = sprite;
        if (direction == Vector2.left)
        {
            spriteRenderer.flipX = true;
        }

        obj.transform.parent = parentObj.transform;
    }

    /// <summary>
    /// Get a random speed, if it's equal to the previous lane's speed
    /// then recursively call it again, otherwise keep it and reassign 
    /// previous lane speed to it.
    /// </summary>
    /// <param name="x"> Min Speed </param>
    /// <param name="y"> Max Speed </param>
    private void RandomSpeed(int x, int y)
    {
        laneSpeed = Random.Range(x, y);
        if (laneSpeed == previousLaneSpeed)
        {
            RandomSpeed(x, y);
        }
        previousLaneSpeed = laneSpeed;
    }

    /// <summary>
    /// Destroys all currently existing objects on the map,
    /// Generates first sidewalk+barrier and a specified amount of lanes.
    /// </summary>
    public void ResetMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateBarrier(-2);
        GenerateSidewalk(0);
        Generate((int)Player.Instance.transform.position.y+1, lanes);
    }

    /// <summary>
    /// Returns A Random Direction. (Left/Right)
    /// </summary>
    /// <returns> Vector2 Left or Right </returns>
    private Vector2 GetDirection()
    {
        Vector2[] directions = { Vector2.left, Vector2.right };
        return directions[Random.Range(0, 2)];
    }

    /// <summary>
    /// Calculates the available spawn locations so that objects 
    /// don't overlap based on the available space and amount of objects.
    /// </summary>
    /// <param name="n"> Amount Of Objects </param>
    /// <returns> Returns An Array of Spawn Points </returns>
    private int[] CalculateSpawnPoints(int n)
    {
        int[] spArray = new int[n + 1]; //Spawn Points
        spArray[0] = 1;
        for (int k = 1; k < spArray.Length; k++)
        {
            spArray[k] = k * (LANE_WIDTH / n);
        }
        return spArray;
    }

    private GameObject SpawnObj(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj = Instantiate(obj, position, rotation);
        obj.transform.parent = this.transform;
        return obj;
    }

    private GameObject SpawnObj(GameObject obj, Vector3 position, Quaternion rotation, GameObject parent)
    {
        obj = Instantiate(obj, position, rotation);
        obj.transform.parent = parent.transform;
        return obj;
    }

    public void LoadLevel(string filename)
    {
        LevelState levelState = LevelStateManager.Instance.LoadLevelState(filename);
        if (levelState != null)
        {
            // Empty Existing Map.
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                DestroyImmediate(child);
            }
            // Load Player Info.
            Player.Instance.blockDistance = levelState.playerInfo.blockDistance;
            Player.Instance.obstacleImmunity = levelState.playerInfo.obstacleImmunity;
            Player.Instance.transform.position = levelState.playerInfo.position;
            Player.Instance.transform.rotation = levelState.playerInfo.rotation;
            // Generation Info.
            i = levelState.generationInfo.laneCount;
            // Obstacle Info.
            foreach (RoadPieceState roadPieceState in levelState.roadPieces)
            {
                GameObject roadPiece = SpawnObj(GetTypeByTag(roadPieceState.type), roadPieceState.position, roadPieceState.rotation);
                foreach (MovingChildState movingChildState in roadPieceState.movingChildren)
                {
                    GameObject movingChild = Instantiate(GetTypeByTag(movingChildState.type), movingChildState.position, movingChildState.rotation, roadPiece.transform);
                    if (movingChildState.type == "Car")
                    {
                        Sprite sprite = GetSpriteByType(roadPieceState.type);
                        movingChild.GetComponent<SpriteRenderer>().sprite = sprite;
                        if (movingChildState.direction == new Vector3(-1,0,0))
                        {
                            movingChild.GetComponent<SpriteRenderer>().flipX = true;
                        }
                    }
                    movingChild.GetComponent<Obstacle>().direction = movingChildState.direction;
                    movingChild.GetComponent<Obstacle>().speed = movingChildState.speed;
                    movingChild.GetComponent<Obstacle>().size = movingChildState.size;
                }
            }
        }
    }

    private GameObject GetTypeByTag(string tag)
    {
        switch (tag)
        {
            case "Sidewalk":
                return sidewalk;
            case "Barrier":
                return barrier;
            case "LavaRoad":
                return lavaRoad;
            case "ReverseRoad":
                return reverseRoad;
            case "Road":
                return road;
            case "Water":
                return water;
            case "Car":
                return car;
            case "Turtle":
                return turtle;
            case "Log":
                return log;
            case "Powerup":
                return powerUp;
            case "Star":
                return star;
            default:
                return null;
        }
    }

    private Sprite GetSpriteByType(string type)
    {
        switch (type)
        {
            case "LavaRoad":
                return tractorSprites[Random.Range(0, tractorSprites.Length)];
            case "ReverseRoad":
                return tractorSprites[Random.Range(0, tractorSprites.Length)];
            case "Road":
                return carSprites[Random.Range(0, carSprites.Length)];
            default:
                return null;
        }
    }
}
