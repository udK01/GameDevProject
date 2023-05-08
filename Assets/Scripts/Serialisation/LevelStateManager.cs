using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelStateManager : MonoBehaviour
{

    [SerializeField] GameObject proceduralLevel;
    public static LevelStateManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public LevelState GetLevelState()
    {
        LevelState levelState = new LevelState();
        levelState.roadPieces = new List<RoadPieceState>();

        // Player
        levelState.playerInfo = new PlayerInfo();
        levelState.playerInfo.blockDistance = Player.Instance.blockDistance;
        levelState.playerInfo.obstacleImmunity = Player.Instance.obstacleImmunity;
        levelState.playerInfo.position = Player.Instance.transform.position;
        levelState.playerInfo.rotation = Player.Instance.transform.rotation;

        // Generation
        levelState.generationInfo = new GenerationInfo();
        levelState.generationInfo.laneCount = ProceduralGeneration.Instance.i;

        // Objects
        foreach (Transform child in proceduralLevel.transform)
        {
            RoadPieceState roadPiece = new RoadPieceState();
            roadPiece.type = child.tag;
            roadPiece.position = child.transform.position;
            roadPiece.rotation = child.transform.rotation;
            foreach (Transform child2 in child.transform)
            {
                if (!child2.name.Contains("Barrier"))
                {
                    MovingChildState movingChild = new MovingChildState();
                    movingChild.type = child2.tag;
                    movingChild.position = child2.transform.position;
                    movingChild.rotation = child2.transform.rotation;
                    movingChild.direction = child2.GetComponent<Obstacle>().direction;
                    movingChild.speed = child2.GetComponent<Obstacle>().speed;
                    movingChild.size = child2.GetComponent<Obstacle>().size;
                    roadPiece.movingChildren.Add(movingChild);
                }
            }
            levelState.roadPieces.Add(roadPiece);
        }
        return levelState;
    }

    public void SaveLevelState(LevelState levelState, string filename)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        string path = Application.persistentDataPath + "/" + filename;

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, levelState);
        }
    }

    public LevelState LoadLevelState(string filename)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        string path = Application.persistentDataPath + "/" + filename;

        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return (LevelState)formatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("File not found: " + path);
            return null;
        }
    }

    public BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
