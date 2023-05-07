using UnityEngine;

public class CollectionAchievements : AchievementType
{
    private float collectionThreshold;
    private int collectionType; // Type 1 = Powerup, Type 2 = Star

    public CollectionAchievements(string name, string description, float threshold, int type) : base(name, description)
    {
        collectionThreshold = threshold;
        collectionType = type;
    }

    public bool CheckCollectionThreshold(float count)
    {
        if (count >= collectionThreshold && !isCompleted)
        {
            isCompleted = true;
            PlayerPrefs.SetInt(achievementName, 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CheckIfAchievementEarned()
    {
        switch (collectionType)
        {
            case 1:
                return CheckCollectionThreshold(GameManager.Instance.powerUpCount);
            case 2:
                return CheckCollectionThreshold(GameManager.Instance.starCount);
            default:
                return false;
        }
    }
}
