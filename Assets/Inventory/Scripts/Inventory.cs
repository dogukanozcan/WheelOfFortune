using Naku.WheelOfFortune;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Naku.InventorySystem
{
    public class Inventory : MonoSingleton<Inventory>
    {
        private string inventoryPath;
        private InventoryData m_inventory = new InventoryData();

        public event Action<List<Item>> OnInventoryChanged;

        private void Awake()
        {
            inventoryPath = Application.persistentDataPath + "/Inventory.json";
        }
        private void Start()
        {
            //load database
            LoadAll();
        }
        public void AddItem(ItemStack itemStack)
        {
            //Convert itemSO to Item
            var item = new Item(itemStack);

            //Create inventory if its null
            m_inventory ??= new InventoryData();

            //Create list if its null
            m_inventory.list ??= new List<Item>();

            //Check if item exists before adding it 
            var checkItem = m_inventory.list.Find(a => a == item);
            if (checkItem != null)
            {
                //if exist increase stack count
                checkItem.StackCount += item.StackCount;
            }
            else
            {
                // if not exist add to list
                m_inventory.list.Add(item);
            }

            //Update Database
            UpdateSave();
            //Call OnInventoryChanged
            OnInventoryChanged?.Invoke(m_inventory.list);
        }
        public void RemoveItem(Item item)
        {
            //Check if there is an item to delete
            var checkItem = m_inventory.list.Find(a => a == item);
            if (checkItem == null)
            {
                Debug.LogError("The item to be removed could not be found.");
                return;
            }

            //if stack count is more than 1
            if (checkItem.StackCount > 1)
            {
                //decrease stack count
                checkItem.StackCount--;
            }
            else
            {
                //if stack count is equal to 1 remove item from list
                m_inventory.list.Remove(checkItem);
            }

            //Update Database
            UpdateSave();
            //Call OnInventoryChanged
            OnInventoryChanged?.Invoke(m_inventory.list);
        }

        public void UpdateSave()
        {
            //Convert inventory to json and write to file system
            var json = JsonUtility.ToJson(m_inventory);
            File.WriteAllText(inventoryPath, json);
        }

        public void LoadAll()
        {
            if (File.Exists(inventoryPath))
            {
                //Read file
                string loadPlayerData = File.ReadAllText(inventoryPath);
                //Convert json to inventory
                m_inventory = JsonUtility.FromJson<InventoryData>(loadPlayerData);
                //Call OnInventoryChanged
                OnInventoryChanged?.Invoke(m_inventory.list);
            }
        }
    }
}