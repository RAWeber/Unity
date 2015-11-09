using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ItemDatabase : MonoBehaviour
{

    public TextAsset itemInventory;
    public static List<BaseItem> itemDatabase = new List<BaseItem>();

    private List<Dictionary<string, string>> databaseDictionary = new List<Dictionary<string, string>>();
    private Dictionary<string, string> itemDictionary;

    void Awake()
    {
        ReadItemDatabase("Item");
    }

    private void ReadItemDatabase(string type)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(itemInventory.text);
        XmlNodeList itemList = xmlDocument.GetElementsByTagName(type);

        foreach (XmlNode item in itemList)
        {
            XmlNodeList itemContents = item.ChildNodes;
            itemDictionary = new Dictionary<string, string>();

            foreach (XmlNode content in itemContents)
            {
                switch (content.Name)
                {
                    case "ItemID":
                        itemDictionary.Add("ItemID", content.InnerText);
                        break;
                    case "ItemName":
                        itemDictionary.Add("ItemName", content.InnerText);
                        break;
                    case "ItemType":
                        itemDictionary.Add("ItemType", content.InnerText);
                        break;
                    case "SubType":
                        itemDictionary.Add("SubType", content.InnerText);
                        break;
                    case "MaxSize":
                        itemDictionary.Add("MaxSize", content.InnerText);
                        break;
                    case "Description":
                        itemDictionary.Add("Description", content.InnerText);
                        break;
                }
            }
            databaseDictionary.Add(itemDictionary);
        }
        for (int i = 0; i < databaseDictionary.Count; i++)
        {
            switch (databaseDictionary[i]["ItemType"])
            {
                case "WEAPON":
                    itemDatabase.Add(new BaseWeapon(databaseDictionary[i]));
                    break;
                case "CONSUMABLE":
                    itemDatabase.Add(new BaseConsumable(databaseDictionary[i]));
                    break;
                case "EQUIPMENT":
                    itemDatabase.Add(new BaseEquipment(databaseDictionary[i]));
                    break;
                case "RELIC":
                    itemDatabase.Add(new BaseRelic(databaseDictionary[i]));
                    DropManager.relicNames.Add(databaseDictionary[i]["ItemName"]);
                    break;
            }
            //itemDatabase.Add(new BaseItem(databaseDictionary[i]));
        }
    }
}
