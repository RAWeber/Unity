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
        ReadItemDatabase();
    }

    private void ReadItemDatabase()
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(itemInventory.text);
        XmlNodeList itemList = xmlDocument.GetElementsByTagName("Item");

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
                }
            }
            databaseDictionary.Add(itemDictionary);
        }
        for (int i = 0; i < databaseDictionary.Count; i++)
        {
            
            itemDatabase.Add(new BaseItem(databaseDictionary[i]));
        }
    }
}
