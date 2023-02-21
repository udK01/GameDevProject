using UnityEngine;

public enum Options { ROAD, SIDEWALK, WATER }

public class ProceduralGeneration : MonoBehaviour
{
    private Options option;
    private int i;
    private int ignoreSideWalk = 1;
    [SerializeField] int lanes;

    [SerializeField] GameObject road;
    [SerializeField] GameObject sidewalk;
    [SerializeField] GameObject water;
    [SerializeField] GameObject barrier;

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
                    GenerateRoad(i);
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
                    GenerateWater(i);
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
            i++;
        }
        i--;
    }

    private void GenerateWater(int y)
    {
        for (int j = y; j < (y+Random.Range(minWaterSize, maxWaterSize)); j++)
        {
            SpawnObj(water, 0, j);
            i++;
        }
        i--;
    }

    private void GenerateSidewalk(int y)
    {
        SpawnObj(sidewalk, 0, y);
        ignoreSideWalk = 1;
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



}
