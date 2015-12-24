using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public static Player player;
    public static Inventory inventory;
    public static EquipmentWindow equipment;
    public static CombinationWindow comboWindow;
    public static Canvas canvas;

    public static List<PlayerSave> playerSaves;
    public static PlayerSave currentPlayer;

	// Use this for initialization
	void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        Debug.Log("Saving " + currentPlayer.playerName + "'s game data");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + currentPlayer.playerName + ".dat");

        GameData data = new GameData();
        data.playerInfo = player.SaveInfo();
        //data.inventorySlots = inventory.SaveInfo();
        //data.equipmentSlots = equipment.SaveInfo();
        //data.comboSlots = comboWindow.SaveInfo();


        bf.Serialize(file, data);
        file.Close();

        //SaveStartInfo();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/"+ currentPlayer.playerName + ".dat"))
        {
            Debug.Log("Loading " + currentPlayer.playerName + "'s game data");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + currentPlayer.playerName + ".dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            player.LoadInfo(data.playerInfo);
            inventory.LoadInfo(data.inventorySlots);
            equipment.LoadInfo(data.equipmentSlots);
            comboWindow.LoadInfo(data.comboSlots);
        }
        else
        {
            Debug.Log("No game data for " + currentPlayer.playerName);
        }
    }

    public void SaveStartInfo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/characterSaves.dat");

        StartMenuData data = new StartMenuData();
        data.playerSaves = playerSaves;
        data.playerSaves.Remove(currentPlayer);
        currentPlayer.playerLevel = player.Level;
        currentPlayer.scene = Application.loadedLevel;
        //data.playerSaves.Add(currentPlayer);
        data.playerSaves.Insert(0, currentPlayer);

        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
public class GameData
{
    public PlayerData playerInfo;
    public SlotWindowData inventorySlots;
    public SlotWindowData equipmentSlots;
    public SlotWindowData comboSlots;
}
