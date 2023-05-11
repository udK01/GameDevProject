public class DeathAchievements : AchievementType
{
    private float deathThreshold;
    private int deathType; // Type 1 = Car, Type 2 = Water, Type 3 = Near Death

    public DeathAchievements(string name, string description, float threshold, int type) : base(name, description)
    {
        deathThreshold = threshold;
        deathType = type;
    }

    protected override bool CheckCompletionCondition()
    {
        switch (deathType)
        {
            case 1:
                return GameManager.Instance.carDeathCount >= deathThreshold;
            case 2:
                return GameManager.Instance.waterDeathCount >= deathThreshold;
            case 3:
                return GameManager.Instance.nearDeathCount >= deathThreshold;
            default:
                return false;
        }
    }
}