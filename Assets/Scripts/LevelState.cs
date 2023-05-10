using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelState
{
    public GenerationInfo generationInfo;
    public PlayerInfo playerInfo;
    public List<RoadPieceState> roadPieces;
}

[System.Serializable]
public class GenerationInfo
{
    public int laneCount;
}

[System.Serializable]
public class PlayerInfo
{
    public bool obstacleImmunity;
    public int blockDistance;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class RoadPieceState
{
    public string type;
    public Vector3 position;
    public Quaternion rotation;
    public List<MovingChildState> movingChildren = new List<MovingChildState>();
}

[System.Serializable]
public class MovingChildState
{
    public string type;
    public Vector3 position;
    public Quaternion rotation;
    public float speed;
    public double size;
    public Vector3 direction;
}


