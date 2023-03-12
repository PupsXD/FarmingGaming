using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Creature")]
public class Creature : Item
{
    [SerializeField] private EntityData _entityData;
    [SerializeField] private Food _prefferedFood;
    [SerializeField] private int _requiredFoodAmountToFeed;
    [SerializeField] private GameObject _worldCreatureModel;
    public EntityData EntityData
    {
        get { return _entityData; }
    }
    public Food PrefferedFood
    {
        get { return _prefferedFood; }
    }
    public int RequiredFoodAmountToFeed
    {
        get { return _requiredFoodAmountToFeed;}
    }
    public GameObject WorldCreatureModel
    {
        get { return _worldCreatureModel; }
    }
}
