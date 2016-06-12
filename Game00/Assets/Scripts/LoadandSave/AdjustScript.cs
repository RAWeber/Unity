using UnityEngine;
using System.Collections;

public class AdjustScript : MonoBehaviour {

	void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 80, 100, 30), "Health up"))
        //{
        //    GameControl.player.Stats.AddStat<LinkableStat>(StatType.HEALTH, 10);
        //}
        //if (GUI.Button(new Rect(10, 120, 100, 30), "Health down"))
        //{
        //    GameControl.player.Stats.RemoveStat<LinkableStat>(StatType.HEALTH, 10);
        //}
        //if (GUI.Button(new Rect(10, 160, 100, 30), "Str up"))
        //{
        //    GameControl.player.Stats.AddStat<ModifiableStat>(StatType.STRENGTH, 10);
        //}
        //if (GUI.Button(new Rect(10, 200, 100, 30), "Str down"))
        //{
        //    GameControl.player.Stats.RemoveStat<ModifiableStat>(StatType.STRENGTH, 10);
        //}
        if (GUI.Button(new Rect(10, 240, 100, 30), "Save"))
        {
            GameControl.control.Save();
        }
        if (GUI.Button(new Rect(10, 280, 100, 30), "Load"))
        {
            GameControl.control.Load();
        }
    }
}
