using Naku.WheelOfFortune;
using System;

namespace Naku.InventorySystem
{

    [Serializable]
    public class Item
    {
        public int id;
        public int StackCount = 1;

         private ItemSO itemSO;
         public ItemSO ItemSO
        {
             get
             {
                 if (!itemSO)
                 {
                    itemSO = ItemDatabase.GetItem(id);
                 }
                 return itemSO;
             }
             set
             {
                itemSO = value;
             }
         }

        public Item() { }

        public Item(ItemStack item)
        {
            id = item.ItemSO.Id;
            itemSO = item.ItemSO;
            StackCount = item.StackCount;
        }

        public static bool operator == (Item lhs, Item rhs)
        {
            return lhs?.id == rhs?.id;
        }
        public static bool operator != (Item lhs, Item rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return itemSO.ItemName;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}