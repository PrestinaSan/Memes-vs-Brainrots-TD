using System;
using UnityEngine;


[Serializable]
public class Tower
{
    public int index;
    public string name;
    public int cost;
    public int upgradeCost;
    public string type;
    public GameObject prefab;
    public string description;
    public int nextUpgrade;

    public Tower(int _index, string _name, int _cost, int _upgradeCost,string _type, GameObject _prefab, string _description, int _nextUpgrade)
    {
        index = _index;
        name = _name; 
        cost = _cost;
        upgradeCost = _upgradeCost;
        type = _type;
        prefab = _prefab;
        description = _description;
        nextUpgrade = _nextUpgrade;
    }
}
