using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public List<AchievementType> achievements = new List<AchievementType>();
    [SerializeField] private Text achievementTitleText;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private GameObject content;

    private Queue<AchievementType> notificationQueue = new Queue<AchievementType>();

    private void Start()
    {
        // Distance Achievements
        achievements.Add(new DistanceAchievement("On Your Way", "Travel 10 blocks", 10f));
        achievements.Add(new DistanceAchievement("Hopping Along", "Travel 25 blocks", 25f));
        achievements.Add(new DistanceAchievement("Croak Crossing Crusader", "Travel 50 blocks", 50f));
        achievements.Add(new DistanceAchievement("Leaping Legend", "Travel 75 blocks", 75f));
        achievements.Add(new DistanceAchievement("Hundred-Block Hopper", "Travel 100 blocks", 100f));
        achievements.Add(new DistanceAchievement("Milestone Mover", "Travel 250 blocks", 250f));
        achievements.Add(new DistanceAchievement("Road Warrior", "Travel 500 blocks", 500f));
        achievements.Add(new DistanceAchievement("Croak Crossing Champion", "Travel 1000 blocks", 1000f));
        // Collection Achievements
        // Power-Ups
        achievements.Add(new CollectionAchievements("Empowered!", "Collect 1 Power-Up", 1f, 1));
        achievements.Add(new CollectionAchievements("Power Surge!", "Collect 5 Power-Ups", 5f, 1));
        achievements.Add(new CollectionAchievements("Powerhouse!", "Collect 10 Power-Ups", 10f, 1));
        achievements.Add(new CollectionAchievements("Power Crazed!", "Collect 25 Power-Ups", 25f, 1));
        achievements.Add(new CollectionAchievements("Power-Up Junkie!", "Collect 50 Power-Ups", 50f, 1));
        achievements.Add(new CollectionAchievements("Supercharged!", "Collect 50 Power-Ups", 100f, 1));
        // Stars
        achievements.Add(new CollectionAchievements("Starstruck!", "Collect 1 Star", 1f, 2));
        achievements.Add(new CollectionAchievements("Shining Bright!", "Collect 3 Stars", 3f, 2));
        achievements.Add(new CollectionAchievements("Star Chaser!", "Collect 6 Stars", 6f, 2));
        achievements.Add(new CollectionAchievements("Star Collector!", "Collect 12 Stars", 12f, 2));
        achievements.Add(new CollectionAchievements("Galatic Explorer!", "Collect 25 Stars", 25f, 2));
        achievements.Add(new CollectionAchievements("Starforger!", "Collect 50 Stars", 50f, 2));
        // Car Death
        achievements.Add(new DeathAchievements("Road-Kill!", "Get Hit By A Car", 1f, 1));
        achievements.Add(new DeathAchievements("Leap of Miscalculations!", "Get Hit 5 Times", 5f, 1));
        achievements.Add(new DeathAchievements("Froggy Crash Test!", "Get Hit 10 Times", 10f, 1));
        achievements.Add(new DeathAchievements("Jumpy Accident Magnet", "Get Hit 25 Times", 25f, 1));
        achievements.Add(new DeathAchievements("Master of Frogger Disasters!", "Get Hit 50 Times", 50f, 1));
        //  Water Death
        achievements.Add(new DeathAchievements("Splashdown!", " Drown Once", 1f, 2));
        achievements.Add(new DeathAchievements("Aqua Fiasco!", "Drown 5 Times", 5f, 2));
        achievements.Add(new DeathAchievements("Froggy Deep-Sea Explorer!", "Drown 10 Times", 10f, 2));
        achievements.Add(new DeathAchievements("Submerged Hopper", "Drown 25 Times", 25f, 2));
        achievements.Add(new DeathAchievements("Amphibious Sinking Expert!", "Drown 50 Times", 50f, 2));
        // Close Calls
        achievements.Add(new DeathAchievements("Close Call!", "Narrowly Avoid a Car Once", 1f, 3));
        achievements.Add(new DeathAchievements("Froggy Reflexes!", "Narrowly Avoid a Car 3 Times", 3f, 3));
        achievements.Add(new DeathAchievements("Roadside Acrobat!", "Narrowly Avoid a Car 6 Times", 6f, 3));
        achievements.Add(new DeathAchievements("Master of Frogger Evasion!", "Narrowly Avoid a Car 12 Times", 12f, 3));
        achievements.Add(new DeathAchievements("Daredevil!", "Narrowly Avoid a Car 25 Times", 25f, 3));

        // This is for testing purposes.
        //foreach (AchievementType achievement in achievements)
        //{
        //    if (PlayerPrefs.HasKey(achievement.achievementName))
        //    {
        //        PlayerPrefs.DeleteKey(achievement.achievementName);
        //    }
        //}

        foreach (AchievementType achievement in achievements)
        {
            achievement.isCompleted = PlayerPrefs.GetInt(achievement.achievementName, 0) == 1;
        }
    }

    private void Update()
    {
        foreach (AchievementType achievement in achievements)
        {
            if (achievement.CheckIfAchievementEarned())
            {
                notificationQueue.Enqueue(achievement);
            }
        }

        if (notificationQueue.Count > 0)
        {
            AchievementType nextAchievement = notificationQueue.Dequeue();
            ShowNotification(nextAchievement);
        }
    }

    public void ShowNotification(AchievementType achievement)
    {
        achievementTitleText.text = achievement.achievementName;
        anim.SetTrigger("Appear");
    }

    public void DisplayAchievements()
    {
        ClearScrollViewContent();
        foreach (AchievementType achievement in achievements)
        {
            GameObject achievementDisplay = Instantiate(achievementPrefab, Vector3.zero, Quaternion.identity);
            GameObject iconUnlocked = achievementDisplay.transform.Find("IconContainer").Find("IconUnlocked").gameObject;
            GameObject achievementName = achievementDisplay.transform.Find("AchievementContent").Find("AchievementName").gameObject;
            GameObject achievementDescription = achievementDisplay.transform.Find("AchievementContent").Find("AchievementDescription").gameObject;
            if (achievement.isCompleted)
            {
                iconUnlocked.SetActive(true);
            } else
            {
                iconUnlocked.SetActive(false);
            }
            achievementName.GetComponent<Text>().text = achievement.achievementName;
            achievementDescription.GetComponent<Text>().text = achievement.achievementDescription;
            achievementDisplay.transform.SetParent(content.transform, false);
        }
    }

    private void ClearScrollViewContent()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
