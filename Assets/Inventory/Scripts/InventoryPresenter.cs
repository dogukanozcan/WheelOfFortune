using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naku.InventorySystem
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private Transform cellParent;
        [SerializeField] private ItemCell cellPrefab;

        private List<ItemCell> m_inventoryCells;

        private void Awake()
        {
            m_inventoryCells = cellParent.GetComponentsInChildren<ItemCell>().ToList();
        }

        private void OnEnable()
        {
            Inventory.Instance.OnInventoryChanged += OnInventoryChanged;
        }
        private void OnDisable()
        {
            if (Inventory.Instance)
                Inventory.Instance.OnInventoryChanged -= OnInventoryChanged;
        }
        private void OnInventoryChanged(List<Item> inventory)
        {
            //Empty all cells
            foreach (var cell in m_inventoryCells)
            {
                cell.SetEmpty();
            }

            //FULL INVENTORY CHECK
            var diff = inventory.Count - m_inventoryCells.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    //if inventory doesn't fit add cell
                    AddCellToInventory();
                }
            }

            //Assign inventory
            foreach (var item in inventory)
            {
                GetNextCell.AttachItem(item);
            }

        }

        public void AddCellToInventory()
        {
            var cell = Instantiate(cellPrefab, cellParent);
            m_inventoryCells.Add(cell);
        }

        private bool IsInventoryFull => !m_inventoryCells.Any(cell => cell.IsEmpty());
        private ItemCell GetNextCell => m_inventoryCells.Find(cell => cell.IsEmpty());
    }
}