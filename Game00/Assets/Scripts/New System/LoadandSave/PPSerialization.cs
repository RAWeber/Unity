using UnityEngine;
//using System.Collections;
using System.IO;
//using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class PPSerialization {

    public static BinaryFormatter binaryFormatter = new BinaryFormatter();

    public static void Save(string saveTag, object obj)
    {
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        string temp = System.Convert.ToBase64String(memoryStream.ToArray());
        PlayerPrefs.SetString(saveTag, temp);
    }

    public static object Load(string saveTag)
    {
        string temp = PlayerPrefs.GetString(saveTag);
        if (temp == string.Empty)
        {
            return null;
        }
        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(temp));
        return binaryFormatter.Deserialize(memoryStream);
    }

    public static T Clone<T>(T source)
    {
        if (!typeof(T).IsSerializable) throw new ArgumentException("The type must be serializable", "source");

        if (System.Object.ReferenceEquals(source, null)) return default(T);

        Stream stream = new MemoryStream();
        using (stream)
        {
            binaryFormatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)(binaryFormatter.Deserialize(stream));
        }
    }
}
