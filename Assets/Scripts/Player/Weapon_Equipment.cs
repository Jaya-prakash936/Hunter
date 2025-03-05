using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Equipment : MonoBehaviour
{
    [SerializeField] GameObject WeaponHolder;
    [SerializeField] GameObject Weapon;
    [SerializeField] GameObject WeaponSheath;

    GameObject CurrentWeaponInHand;
    GameObject CurrentWeaponInSheath;
    public bool WeaponEquiped = false;
    
    void Start()
    {
        { 
        CurrentWeaponInSheath = Instantiate(Weapon, WeaponSheath.transform);
            WeaponEquiped = false;
        }
    }

    public void DrawWeaponn()
    {
        CurrentWeaponInHand = Instantiate(Weapon, WeaponHolder.transform);
        Destroy(CurrentWeaponInSheath);
        WeaponEquiped = true;
    }

    public void SheathWeaponn()
    {
        CurrentWeaponInSheath=Instantiate(Weapon, WeaponSheath.transform);
        Destroy(CurrentWeaponInHand);
        WeaponEquiped = false;
    }

    public void StartDealDamage()
    {
        CurrentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        CurrentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
