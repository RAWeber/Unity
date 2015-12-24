using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class ItemDatabase : MonoBehaviour
{
    public static Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem>(); //Dictionary containing all game items
    public static Dictionary<HashSet<string>, BaseItem> componentDatabase = new Dictionary<HashSet<string>, BaseItem>(new HashSetEqualityComparer<string>()); //Dictionary containing relics needed to create each item
    public static List<string> relicNames = new List<string>();

    public TextAsset itemDatabaseSpreadSheet; //XML file

    private HashSet<string> componentRelics; //relics needed to create item

    void Start()
    {
        ReadItemSpreadSheet("Item");
    }

    private void ReadItemSpreadSheet(string type)
    {
        //Load XML File
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(itemDatabaseSpreadSheet.text);
        XmlNodeList itemList = xmlDocument.GetElementsByTagName(type);

        componentRelics = new HashSet<string>();

        //Adds each item to dictionary
        foreach (XmlNode item in itemList)
        {
            Dictionary<string,string> itemData = CreateItemDictionary(item);
            AddItemsToDatabase(itemData);
            componentRelics = new HashSet<string>();
        }
    }

    //Creates a single item dictionary based on info inside XML Node
    private Dictionary<string,string> CreateItemDictionary(XmlNode item)
    {
        XmlNodeList itemContents = item.ChildNodes;
        Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

        foreach (XmlNode content in itemContents)
        {
            if (!content.InnerText.Equals(""))
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
                    case "Quality":
                        itemDictionary.Add("Quality", content.InnerText);
                        break;
                    case "MaxSize":
                        itemDictionary.Add("MaxSize", content.InnerText);
                        break;
                    case "Description":
                        itemDictionary.Add("Description", content.InnerText);
                        break;
                    case "BaseStat1":
                        itemDictionary.Add("BaseStat1", content.InnerText);
                        break;
                    case "BaseStat2":
                        itemDictionary.Add("BaseStat2", content.InnerText);
                        break;
                    case "BaseStat3":
                        itemDictionary.Add("BaseStat3", content.InnerText);
                        break;
                    case "BaseStat4":
                        itemDictionary.Add("BaseStat4", content.InnerText);
                        break;
                    case "Relic01":
                    case "Relic02":
                    case "Relic03":
                    case "Relic04":
                    case "Relic05":
                        if (!content.InnerText.Equals(""))
                        {
                            componentRelics.Add(content.InnerText); ;
                        }
                        break;
                    default:
                        Debug.Log("[ItemDataBase] XML reading error");
                        break;
                }
            
            }
        }
        return itemDictionary;
    }

    //Creates an item to be added into the database based on single item dictionary
    private void AddItemsToDatabase(Dictionary<string,string> itemData)
    {
        switch (itemData["ItemType"])
        {
            case "WEAPON":
                itemDatabase.Add(itemData["ItemName"], new BaseWeapon(itemData));
                componentDatabase.Add(componentRelics, new BaseWeapon(itemData));
                break;
            case "CONSUMABLE":
                itemDatabase.Add(itemData["ItemName"], new BaseConsumable(itemData));
                break;
            case "EQUIPMENT":
                itemDatabase.Add(itemData["ItemName"], new BaseEquipment(itemData));
                componentDatabase.Add(componentRelics, new BaseEquipment(itemData));
                break;
            case "RELIC":
                itemDatabase.Add(itemData["ItemName"], new BaseRelic(itemData));
                relicNames.Add(itemData["ItemName"]);
                break;
            default:
                Debug.Log("[ItemDatabase] " + itemData["ItemName"] + " failed to be added to itemDatabase");
                break;
        }
    }
}
