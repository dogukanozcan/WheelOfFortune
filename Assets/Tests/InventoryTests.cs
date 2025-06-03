using NUnit.Framework;
using UnityEngine;
using Naku.InventorySystem;
using System.Reflection;

public class InventoryTests
{
    [Test]
    public void AddItemTwice_IncrementsStack()
    {
        var go = new GameObject();
        var inventory = go.AddComponent<Inventory>();

        var itemSO = ScriptableObject.CreateInstance<ItemSO>();
        typeof(ItemSO).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .SetValue(itemSO, 1, null);
        typeof(ItemSO).GetProperty("ItemName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .SetValue(itemSO, "Test Item", null);

        var stack1 = new ItemStack(itemSO, 1);
        var stack2 = new ItemStack(itemSO, 1);

        inventory.AddItem(stack1);
        inventory.AddItem(stack2);

        var dataField = typeof(Inventory).GetField("m_inventory", BindingFlags.NonPublic | BindingFlags.Instance);
        var data = (InventoryData)dataField.GetValue(inventory);

        Assert.AreEqual(1, data.list.Count);
        Assert.AreEqual(2, data.list[0].StackCount);
    }
}

