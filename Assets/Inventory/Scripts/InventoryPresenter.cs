using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naku.InventorySystem
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private Transform m_cellParent;
        [SerializeField] private ItemCell m_cellPrefab;

        private List<ItemCell> m_inventoryCells;

        private void Awake()
        {
            if(m_cellParent == null)
            {
                Debug.LogError("CellParent not assigned!");
                return;
            }
            m_inventoryCells = m_cellParent.GetComponentsInChildren<ItemCell>().ToList();
        }

        private void OnEnable()
        {
            if (Inventory.Instance)
            {
                Inventory.Instance.OnInventoryChanged += OnInventoryChanged;
            }
            else
            {
                Debug.LogWarning("Inventory Singleton not found!");
            }
                
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
            var cell = Instantiate(m_cellPrefab, m_cellParent);
            m_inventoryCells.Add(cell);
        }

        private bool IsInventoryFull => !m_inventoryCells.Any(cell => cell.IsEmpty());
        private ItemCell GetNextCell => m_inventoryCells.Find(cell => cell.IsEmpty());
    }
}