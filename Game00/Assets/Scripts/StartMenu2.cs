using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;

public class StartMenu2 : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas deleteMenu;
    public Button startButton;
    public Animation startAnimation;
    public Button optionButton;
    public Animation optionAnimation;
    public Button exitButton;
    public Animation exitAnimation;
    public Button backButton;
    public Animation backAnimation;
    public Button newButton;
    public Animation newAnimation;
    public Button loadButton;
    public Animation loadAnimation;
    public Button deleteButton;
    public Animation deleteAnimation;
    public Button leftButton;
    public Animation leftAnimation;
    public Button rightButton;
    public Animation rightAnimation;
    public Button createButton;
    public Animation createAnimation;
    public Button cancelButton;
    public Animation cancelAnimation;
    public Text playerName;
    public Animation playerNameAnimation;
    public Text inputText;
    public Animation inputTextAnimation;

    private int selectedCharacter = 0;

    void Awake()
    {
        Load();
    }

    // Use this for initialization
    void Start () {
        quitMenu.enabled = false;
        deleteMenu.enabled = false;

        if (GameControl.playerSaves.Count != 0)
        {
            playerName.text = GameControl.playerSaves[selectedCharacter].playerName;
        }
    }
	
	public void ExitPress()
    {
        startButton.enabled = false;
        optionButton.enabled = false;
        exitButton.enabled = false;
        quitMenu.enabled = true;
    }

    public void NoExitPress()
    {
        quitMenu.enabled = false;
        startButton.enabled = true;
        optionButton.enabled = true;
        exitButton.enabled = true;
    }

    public void NoDeletePress()
    {
        deleteMenu.enabled = false;
        backButton.enabled = true;
        newButton.enabled = true;
        loadButton.enabled = true;
        deleteButton.enabled = true;
        leftButton.enabled = true;
        rightButton.enabled = true;
    }

    public void StartLevel()
    {
        //SceneManager.LoadScene(1);
        MenuOut();

        newAnimation.Play("EaseIn2");
        loadAnimation.Play("EaseIn2");
        deleteAnimation.Play("EaseIn2");

        leftAnimation.Play("EaseIn3");
        rightAnimation.Play("EaseIn3");
        playerNameAnimation.Play("EaseIn4");
        newButton.enabled = true;
        loadButton.enabled = true;
        deleteButton.enabled = true;
        leftButton.enabled = true;
        rightButton.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MenuOut()
    {
        startButton.enabled = false;
        optionButton.enabled = false;
        exitButton.enabled = false;
        startAnimation.Play("EaseOut");
        optionAnimation.Play("EaseOut");
        exitAnimation.Play("EaseOut");
        backAnimation.Play("EaseIn");
        backButton.enabled = true;
    }

    public void BackButton()
    {
        backButton.enabled = false;
        backAnimation.Play("EaseOut");
        startAnimation.Play("EaseIn");
        optionAnimation.Play("EaseIn");
        exitAnimation.Play("EaseIn");

        newButton.enabled = false;
        loadButton.enabled = false;
        deleteButton.enabled = false;
        leftButton.enabled = false;
        rightButton.enabled = false;
        newAnimation.Play("EaseOut2");
        loadAnimation.Play("EaseOut2");
        deleteAnimation.Play("EaseOut2");

        leftAnimation.Play("EaseOut3");
        rightAnimation.Play("EaseOut3");
        playerNameAnimation.Play("EaseOut4");

        startButton.enabled = true;
        optionButton.enabled = true;
        exitButton.enabled = true;
    }

    public void Left()
    {
        if (GameControl.playerSaves.Count != 0 && --selectedCharacter < 0)
        {
            selectedCharacter = GameControl.playerSaves.Count - 1;
        }
        if (GameControl.playerSaves.Count != 0)
        {
            playerName.text = GameControl.playerSaves[selectedCharacter].playerName;
        }
    }

    public void Right()
    {
        if (GameControl.playerSaves.Count != 0 && ++selectedCharacter >= GameControl.playerSaves.Count)
        {
            selectedCharacter = 0;
        }
        if (GameControl.playerSaves.Count != 0)
        {
            playerName.text = GameControl.playerSaves[selectedCharacter].playerName;
        }
    }

    public void LoadCharacter() {
        if (GameControl.playerSaves.Count != 0)
        {
            GameControl.currentPlayer = GameControl.playerSaves[selectedCharacter];
            GameControl.currentPlayer.loadCharacter = true;
            SceneManager.LoadScene(GameControl.playerSaves[selectedCharacter].scene);
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

    public void NewPress()
    {
        backButton.enabled = false;
        newButton.enabled = false;
        loadButton.enabled = false;
        deleteButton.enabled = false;
        leftButton.enabled = false;
        rightButton.enabled = false;
        backAnimation.Play("EaseOut");
        newAnimation.Play("EaseOut2");
        loadAnimation.Play("EaseOut2");
        deleteAnimation.Play("EaseOut2");
        leftAnimation.Play("EaseOut3");
        rightAnimation.Play("EaseOut3");
        playerNameAnimation.Play("EaseOut4");

        createAnimation.Play("EaseIn2");
        cancelAnimation.Play("EaseIn2");
        inputTextAnimation.Play("EaseIn3");
        createButton.enabled = true;
        cancelButton.enabled = true;
    }

    public void CancelPress()
    {
        createAnimation.Play("EaseOut2");
        cancelAnimation.Play("EaseOut2");
        inputTextAnimation.Play("EaseOut3");
        createButton.enabled = false;
        cancelButton.enabled = false;

        backButton.enabled = true;
        newButton.enabled = true;
        loadButton.enabled = true;
        deleteButton.enabled = true;
        leftButton.enabled = true;
        rightButton.enabled = true;
        backAnimation.Play("EaseIn");
        newAnimation.Play("EaseIn2");
        loadAnimation.Play("EaseIn2");
        deleteAnimation.Play("EaseIn2");
        leftAnimation.Play("EaseIn3");
        rightAnimation.Play("EaseIn3");
        playerNameAnimation.Play("EaseIn4");
    }

    public void CreateCharacter()
    {
        // Username field can't be Default or empty
        if (inputText.text != "" && GameControl.currentPlayer == null)
        {
            foreach (var player in GameControl.playerSaves)
            {
                if (player.playerName == inputText.text)
                {
                    Debug.Log("Character name already exists");
                    return;
                }
            }
            Debug.Log("New character created");
            SceneManager.LoadScene(1);
            GameControl.currentPlayer = new PlayerSave();
            GameControl.currentPlayer.playerName = inputText.text;
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

    public void DeletePress()
    {
        if (GameControl.playerSaves.Count != 0)
        {
            deleteMenu.enabled = true;
            deleteMenu.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Are You Sure You Want To Delete " + GameControl.playerSaves[selectedCharacter].playerName + "?";
            backButton.enabled = false;
            newButton.enabled = false;
            loadButton.enabled = false;
            deleteButton.enabled = false;
            leftButton.enabled = false;
            rightButton.enabled = false;
        }
    }

    public void Delete()
    {
        Debug.Log("Deleting: " + Application.persistentDataPath + "/" + GameControl.playerSaves[selectedCharacter].playerName + ".dat");
        File.Delete(Application.persistentDataPath + "/" + GameControl.playerSaves[selectedCharacter].playerName + ".dat");
        GameControl.playerSaves.RemoveAt(selectedCharacter);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/characterSaves.dat");
        StartMenuData data = new StartMenuData();
        data.playerSaves = GameControl.playerSaves;
        bf.Serialize(file, data);
        file.Close();

        selectedCharacter = 0;
        if (GameControl.playerSaves.Count == 0)
        {
            playerName.text = "";
        }
        else
        {
            playerName.text = GameControl.playerSaves[selectedCharacter].playerName;
        }
        NoDeletePress();
    }
}

[Serializable]
public class StartMenuData
{
    public List<PlayerSave> playerSaves;
}
