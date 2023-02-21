using UnityEngine;

public enum Options { ROAD, SIDEWALK, WATER }

public class ProceduralGeneration : MonoBehaviour
{
    private Options option;
    private int i;

    private const int ignore = 2;
    private const int laneWidth = 14;

    private int ignoreSideWalk = ignore;
    private int ignoreWater = ignore - 1;
    private int ignoreRoad = ignore - 1;
    [SerializeField] int lanes;
    [SerializeField] Sprite[] carSprites;

    [SerializeField] GameObject road;
    [SerializeField] GameObject sidewalk;
    [SerializeField] GameObject water;
    [SerializeField] GameObject barrier;
    [SerializeField] GameObject car;

    [SerializeField] int minRoadSize;
    [SerializeField] int maxRoadSize;
    [SerializeField] int minWaterSize;
    [SerializeField] int maxWaterSize;

    private void Awake()
    {
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
            int speed = Random.Range(1, 4);
            Vector2 direction = GetDirection();
            SpawnObj(road, 0, j);
            GenerateCar(j, Random.Range(1,5), direction, speed);
            ignoreRoad = ignore - 1;
            i++;
        }
        i--;
    }

    private Vector2 GetDirection()
    {
        Vector2[] directions = { Vector2.left, Vector2.right };
        return directions[Random.Range(0, 2)];
    }

    private void GenerateCar(int y, int n, Vector2 direction, float speed)
    {
        Sprite sprite = carSprites[Random.Range(0, 9)];
        int[] spArray = new int[n+1]; //spawn points
        spArray[0] = 1;
        for (int k = 1; k < spArray.Length; k++)
        {
            spArray[k] = k * (laneWidth/n); 
        }
        for (int j = 0; j < n; j++)
        { 
            SpawnObj(car, Random.Range(spArray[j]+1,spArray[j+1]-1), y, direction, speed, 1, sprite);
        }
    }

    private void GenerateWater(int y)
    {
        for (int j = y; j < (y+Random.Range(minWaterSize, maxWaterSize)); j++)
        {
            SpawnObj(water, 0, j);
            ignoreWater = ignore - 1;
            i++;
        }
        i--;
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

    private void SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }

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
