using UnityEngine;

public class DeathAchievements : AchievementType
{
    private float deathThreshold;
    private int deathType; // Type 1 = Car, Type 2 = Water

    public DeathAchievements(string name, string description, float threshold, int type) : base(name, description)
    {
        deathThreshold = threshold;
        deathType = type;
    }

    public bool CheckDeathThreshold(float count)
    {
        if (count >= deathThreshold && !isCompleted)
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
        switch (deathType)
        {
            case 1:
                return CheckDeathThreshold(GameManager.Instance.carDeathCount);
            case 2:
                return CheckDeathThreshold(GameManager.Instance.waterDeathCount);
            //case 3:
            //    return CheckDeathThreshold(GameManager.Instance.nearDeathCount);
            default:
                return false;
        }
    }
}
