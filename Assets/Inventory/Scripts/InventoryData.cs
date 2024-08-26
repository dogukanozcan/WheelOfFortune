using System;
using System.Collections.Generic;

namespace Naku.InventorySystem
{
    [Serializable]
    public class InventoryData
    {
        public List<Item> list = new();
    }
}