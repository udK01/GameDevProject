using UnityEngine;

public abstract class AchievementType
{
    public string achievementName;
    public string achievementDescription;
    public bool isCompleted;

    public AchievementType(string name, string description)
    {
        achievementName = name;
        achievementDescription = description;
        isCompleted = false;
    }

    public bool CheckIfAchievementEarned()
    {
        if (CheckCompletionCondition() && !isCompleted)
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

    protected abstract bool CheckCompletionCondition();

}
