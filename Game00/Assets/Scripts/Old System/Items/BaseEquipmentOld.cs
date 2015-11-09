using UnityEngine;
using System.Collections;

public class BaseEquipmentOld : BaseStatItem {

    public enum EquipmentTypes
    {
        HELM, CHEST, GLOVES, LEGS, BOOTS
    }

    private EquipmentTypes equipmentType;

    public EquipmentTypes EquipmentType { get; set; }
}
