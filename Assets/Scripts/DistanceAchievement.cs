using UnityEngine;

public class DistanceAchievement : AchievementType
{
    public float distanceThreshold;

    public DistanceAchievement(string name, string description, float threshold) : base(name, description)
    {
        distanceThreshold = threshold;
    }

    public override bool CheckIfAchievementEarned()
    {
        if (GameManager.Instance.score >= distanceThreshold && !isCompleted)
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
}
