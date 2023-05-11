public class DistanceAchievement : AchievementType
{
    public float distanceThreshold;

    public DistanceAchievement(string name, string description, float threshold) : base(name, description)
    {
        distanceThreshold = threshold;
    }

    protected override bool CheckCompletionCondition()
    {
        return GameManager.Instance.score >= distanceThreshold;
    }
}