using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public List<DistanceAchievement> distanceAchievements = new List<DistanceAchievement>();
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private Transform achievementListParent;
    [SerializeField] private Text achievementTitleText;
    [SerializeField] private Animator anim;
    // public TimeAchievement timeAchievement;

    // This is for testing purposes.
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        distanceAchievements.Add(new DistanceAchievement("On Your Way", "Travel 10 blocks", 10f));
        distanceAchievements.Add(new DistanceAchievement("Hopping Along", "Travel 25 blocks", 25f));
        distanceAchievements.Add(new DistanceAchievement("Croak Crossing Crusader", "Travel 50 blocks", 50f));
        distanceAchievements.Add(new DistanceAchievement("Leaping Legend", "Travel 75 blocks", 75f));
        distanceAchievements.Add(new DistanceAchievement("Hundred-Block Hopper", "Travel 100 blocks", 100f));
        distanceAchievements.Add(new DistanceAchievement("Milestone Mover", "Travel 250 blocks", 250f));
        distanceAchievements.Add(new DistanceAchievement("Road Warrior", "Travel 500 blocks", 500f));
        distanceAchievements.Add(new DistanceAchievement("Croak Crossing Champion", "Travel 1000 blocks", 1000f));
        // timeAchievement = new TimeAchievement("Speed Demon", "Complete a level in under 30 seconds", 30f);

        foreach (DistanceAchievement achievement in distanceAchievements)
        {
            achievement.isCompleted = PlayerPrefs.GetInt(achievement.achievementName, 0) == 1;
            if (achievement.isCompleted)
            {
                Debug.Log(achievement.achievementName + " is already completed!");
            }
            else
            {
                Debug.Log(achievement.achievementName + " is not completed.");
            }
            //var newAchievementUI = Instantiate(achievementPanel, achievementListParent);
        }
    }

    private void Update()
    {
        foreach (DistanceAchievement achievement in distanceAchievements)
        {
            if (achievement.CheckIfAchievementEarned())
            {
                ShowNotification(achievement);
            }
        }

        //if (timeAchievement.CheckIfAchievementEarned())
        //{
        //    // Award the time achievement to the player
        //    // (e.g. show a pop-up message or play a sound)
        //}

    }

    public void ShowNotification(AchievementType achievement)
    {
        achievementTitleText.text = achievement.achievementName;
        anim.SetTrigger("Appear");
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
