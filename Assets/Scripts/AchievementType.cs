public class AchievementType
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

    public virtual bool CheckIfAchievementEarned()
    {
        return isCompleted;
    }

}

