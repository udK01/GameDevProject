using UnityEngine;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public List<DistanceAchievement> distanceAchievements = new List<DistanceAchievement>();
    // public TimeAchievement timeAchievement;

    void Start()
    {
        distanceAchievements.Add(new DistanceAchievement("On Your Way", "Travel 10 blocks", 10f));
        distanceAchievements.Add(new DistanceAchievement("Hundred-Block Hopper", "Travel 100 blocks", 100f));
        distanceAchievements.Add(new DistanceAchievement("Milestone Mover", "Travel 250 blocks", 250f));
        distanceAchievements.Add(new DistanceAchievement("Road Warrior", "Travel 500 blocks", 500f));
        distanceAchievements.Add(new DistanceAchievement("Croak Crossing Champion", "Travel 1000 blocks", 1000f));
        // timeAchievement = new TimeAchievement("Speed Demon", "Complete a level in under 30 seconds", 30f);

        foreach (DistanceAchievement achievement in distanceAchievements)
        {
            achievement.isCompleted = PlayerPrefs.GetInt(achievement.achievementName, 0) == 1;
        }
    }

    void Update()
    {
        foreach (DistanceAchievement achievement in distanceAchievements)
        {
            if (achievement.CheckIfAchievementEarned())
            {
                // Award the distance achievement to the player
                // (e.g. show a pop-up message or play a sound)
            }
        }

        //if (timeAchievement.CheckIfAchievementEarned())
        //{
        //    // Award the time achievement to the player
        //    // (e.g. show a pop-up message or play a sound)
        //}

    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
