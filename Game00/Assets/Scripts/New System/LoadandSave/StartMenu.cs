using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class StartMenu : MonoBehaviour {

    private int selectedCharacter = 0;
    private string characterName = "Enter name here";
    private string duplicateName;

    void Awake()
    {
        Load();
    }

	void OnGUI()
    {
        GUI.SetNextControlName("MyTextField");
        characterName = GUI.TextField(new Rect(10, 10, 150, 20), characterName);
        //EditorGUI.FocusTextInControl("MyTextField");
        Event e = Event.current;

        // Clear text if default
        if (GUI.GetNameOfFocusedControl() == "MyTextField" && characterName == "Enter name here")
        { 
            characterName = "";
        }else if (GUI.GetNameOfFocusedControl() != "MyTextField" && characterName == "")
        {
            characterName = "Enter name here";
        }

        if (GUI.Button(new Rect(10, 40, 150, 30), "Create new Character") || e.keyCode == KeyCode.Return)
        {
                CreateCharacter();
        }
        if (GUI.Button(new Rect(Screen.width/2-115, Screen.height-40, 30, 30), "<<"))
        {
            if (GameControl.playerSaves.Count!=0 && --selectedCharacter < 0)
            {
                selectedCharacter = GameControl.playerSaves.Count-1;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 40, 150, 30), "Load"))
        {
            if(GameControl.playerSaves.Count != 0)
            {
                GameControl.currentPlayer = GameControl.playerSaves[selectedCharacter];
                GameControl.currentPlayer.loadCharacter = true;
                Application.LoadLevel(GameControl.playerSaves[selectedCharacter].scene);
            }
        }
        if (GUI.Button(new Rect(Screen.width/2 + 85, Screen.height - 40, 30, 30), ">>"))
        {
            if (GameControl.playerSaves.Count != 0 && ++selectedCharacter >= GameControl.playerSaves.Count)
            {
                selectedCharacter = 0;
            }
        }
        if (GameControl.playerSaves.Count != 0)
        {
            GUI.skin.label.alignment = TextAnchor.UpperCenter;
            GUI.skin.label.fontSize = 20;
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 80, 150, 30), GameControl.playerSaves[selectedCharacter].playerName);
            GUI.skin.label.fontSize = 14;
        }
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/characterSaves.dat"))
        {
            Debug.Log("Character list loaded");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/characterSaves.dat", FileMode.Open);

            StartMenuData data = (StartMenuData)bf.Deserialize(file);
            file.Close();

            GameControl.playerSaves = data.playerSaves;
        }
        else
        {
            Debug.Log("Character list empty");
            GameControl.playerSaves = new List<PlayerSave>();
        }
    }

    private void CreateCharacter()
    {
        // Username field can't be Default or empty
        if (characterName != "Enter name here" && characterName != "" && duplicateName != characterName && GameControl.currentPlayer==null)
        {
            foreach(var player in GameControl.playerSaves)
            {
                if(player.playerName == characterName)
                {
                    Debug.Log("Character name already exists");
                    duplicateName = characterName;
                    return;
                }
            }
            Debug.Log("New character created");
            Application.LoadLevel(1);
            GameControl.currentPlayer = new PlayerSave();
            GameControl.currentPlayer.playerName = characterName;
            GameControl.currentPlayer.loadCharacter = false;

            GameControl.currentPlayer.playerLevel = 1;
            GameControl.currentPlayer.scene = 1;
            //GameControl.playerSaves.Add(GameControl.currentPlayer);
            GameControl.playerSaves.Insert(0, GameControl.currentPlayer);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/characterSaves.dat");
            StartMenuData data = new StartMenuData();
            data.playerSaves = GameControl.playerSaves;
            bf.Serialize(file, data);
            file.Close();
        }
    }
}

[Serializable]
public class StartMenuData
{
    public List<PlayerSave> playerSaves;
}