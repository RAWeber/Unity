using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ItemDatabase : MonoBehaviour
{
    public static Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem>();
    public static Dictionary<HashSet<string>, BaseItem> componentDatabase = new Dictionary<HashSet<string>, BaseItem>();

    public TextAsset itemDatabaseSpreadSheet;

    void Awake()
    {
        ReadItemSpreadSheet("Item");
    }

    private void ReadItemSpreadSheet(string type)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(itemDatabaseSpreadSheet.text);
        XmlNodeList itemList = xmlDocument.GetElementsByTagName(type);

        foreach (XmlNode item in itemList)
        {
            Dictionary<string,string> itemData = CreateItemDictionary(item);
            AddItemsToDatabase(itemData);
        }
    }

    private Dictionary<string,string> CreateItemDictionary(XmlNode item)
    {
        XmlNodeList itemContents = item.ChildNodes;
        Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

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
                case "Relics":
                    HashSet<string> components = CreateComponentSet(content.InnerText);
                    //componentDatabase.Add(components, );
                    break;
            }
        }
        return itemDictionary;
    }

    private void AddItemsToDatabase(Dictionary<string,string> itemData)
    {
        switch (itemData["ItemType"])
        {
            case "WEAPON":
                itemDatabase.Add(itemData["ItemName"], new BaseWeapon(itemData));
                break;
            case "CONSUMABLE":
                itemDatabase.Add(itemData["ItemName"], new BaseConsumable(itemData));
                break;
            case "EQUIPMENT":
                itemDatabase.Add(itemData["ItemName"], new BaseEquipment(itemData));
                break;
            case "RELIC":
                itemDatabase.Add(itemData["ItemName"], new BaseRelic(itemData));
                DropManager.relicNames.Add(itemData["ItemName"]);
                break;
        }
    }

    private HashSet<string> CreateComponentSet(string relic)
    {
        HashSet<string> componentRelics = new HashSet<string>();
        string[] relics= relic.Split(',');
        for(int i = 0;i<relics.Length;i++)
        componentRelics.Add(relics[i]);
        return componentRelics;
    }
}
