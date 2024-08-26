using UnityEngine;

namespace Naku.InventorySystem
{
    [System.Serializable]
    public struct ItemStack
    {
        [field: SerializeField]
        public ItemSO ItemSO { get; private set; }

        [field: SerializeField]
        public int StackCount { get; private set; }

        public ItemStack(ItemSO itemSO, int stackCount)
        {
            ItemSO = itemSO;
            StackCount = stackCount;
        }
    }
}