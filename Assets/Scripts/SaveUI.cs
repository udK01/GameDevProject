using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    [SerializeField] GameObject freeSlot;
    [SerializeField] GameObject savedSlot;
    [SerializeField] Text scoreText;
    private string savedScore;

    private void Awake()
    {
        savedScore = name + "score";
        UpdateUI();
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/" + name;
    }

    private bool SaveExists()
    {
        return File.Exists(GetPath());
    }

    public void CreateSave()
    {
        if (ProceduralGeneration.Instance.transform.childCount > 0 && Player.Instance.gameObject.activeSelf)
        {
            scoreText.text = GameManager.Instance.score.ToString();
            PlayerPrefs.SetInt(savedScore, GameManager.Instance.score);
            LevelStateManager.Instance.SaveLevelState(LevelStateManager.Instance.GetLevelState(), name);
        }
        UpdateUI();
    }

    public void LoadSave()
    {
        if (SaveExists())
        {
            ProceduralGeneration.Instance.LoadLevel(name);
            GameManager.Instance.LoadGame(PlayerPrefs.GetInt(savedScore));
        }
    }

    public void DeleteSave()
    {
        File.Delete(GetPath());
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = PlayerPrefs.GetInt(savedScore).ToString();
        if (SaveExists())
        {
            freeSlot.SetActive(false);
            savedSlot.SetActive(true);
        } else
        {
            freeSlot.SetActive(true);
            savedSlot.SetActive(false);
        }
    }
}
