using UnityEngine;
using UnityEngine.UI;
using System;


public class Keybinds : MonoBehaviour
{
    [SerializeField] private string directionString;
    [SerializeField] private int direction;
    [SerializeField] Text keyBind;
    [SerializeField] Text confirmText;

    void Start()
    {
        keyBind.text = PlayerPrefs.GetString(directionString);
    }

    void Update()
    {
        if (keyBind.text == "?")
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keycode))
                {
                    keyBind.text = keycode.ToString();
                    Player.Instance.SetKey(direction, keycode);
                    confirmText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ChangeKey()
    {
        keyBind.text = "?";
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
