public class CollectionAchievements : AchievementType
{
    private float collectionThreshold;
    private int collectionType; // Type 1 = Powerup, Type 2 = Star

    public CollectionAchievements(string name, string description, float threshold, int type) : base(name, description)
    {
        collectionThreshold = threshold;
        collectionType = type;
    }

    protected override bool CheckCompletionCondition()
    {
        switch (collectionType)
        {
            case 1:
                return GameManager.Instance.powerUpCount >= collectionThreshold;
            case 2:
                return GameManager.Instance.starCount >= collectionThreshold;
            default:
                return false;
        }
    }
}
