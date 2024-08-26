using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Naku.InventorySystem
{
    public static class ItemDatabase
    {
        static readonly Dictionary<int, ItemSO> items = new();
        static ItemDatabase()
        {
            items.AddRange(Resources.FindObjectsOfTypeAll<ItemSO>().Select(x => new KeyValuePair<int, ItemSO>(x.Id, x)));
        }
        public static ItemSO GetItem(int id)
        {
            return items[id];
        }
    }
}