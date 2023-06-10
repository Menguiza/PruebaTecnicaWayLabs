using UnityEngine;

namespace UI.Inventory
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        [Header("Info")] 
        public string itemName;
        
        [Header("UI Related")] 
        public Sprite icon;

        [Header("Item Characteristics")] 
        public bool isStackable;
        [Range(1,64)] public int maxStack;
        [Space]
        public bool canBeUsed;

        [Header("Item Parameters")] 
        [Range(0, 100)] public int health;
        [Space] public float speed;
        public float boostDuration;
    }
}