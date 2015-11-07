using UnityEngine;
using System.Collections;

public class CreateNewWeapon : MonoBehaviour {

    private BaseWeapon weapon;
    private int type;
    private int qualityVal;
    private string[] qualities = new string[5] {"COMMON", "GOOD", "GREAT", "RARE", "INSANE"};
    //    private int nameVal = Random.Range(0, 10);
    //    private string[] names = new string[10] { "Sword", "Great Sword", "Longsword", "Rapier", "Broadsword", "Sabre", "Scimitar", "Falcion", "Katana", "Claymore"};

    public void CreateWeapon()
    {
        qualityVal = Random.Range(0, 5);
        type = Random.Range(0, 6);
        weapon = new BaseWeapon();
        weapon.WeaponType = (BaseWeapon.WeaponTypes)type;
        weapon.Name = qualities[qualityVal]+" "+weapon.WeaponType;
        weapon.Description = "I'll figure it out later";
        weapon.Strength = Random.Range(1,11)*qualityVal;
        weapon.Intelligence = Random.Range(1, 11) * qualityVal;
    }

    // Use this for initialization
    void Start()
    {
        CreateWeapon();
        Debug.Log(weapon.Name);
        Debug.Log(weapon.Description);
        Debug.Log(weapon.Strength.ToString());
        Debug.Log(weapon.Intelligence.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
