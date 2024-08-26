using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Naku.InventorySystem
{
    public class ItemCell : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI itemNameLabel;
        [SerializeField] private TextMeshProUGUI itemStackCount;

        private Item attachedItem;
        public void AttachItem(Item item)
        {
            attachedItem = item;
            image.color = Color.white;
            image.sprite = item.ItemSO.ItemSprite;
            itemNameLabel.text = item.ItemSO.ItemName;
            if (item.StackCount > 1) { itemStackCount.text = item.StackCount.ToString(); }

        }

        [ContextMenu("Set Empty")]
        public void SetEmpty()
        {
            attachedItem = null;
            image.color = Color.clear;
            image.sprite = null;
            itemNameLabel.text = "";
            itemStackCount.text = "";
        }

        public bool IsEmpty() => attachedItem == null;

        [ContextMenu("ItemClicked")]
        public void ItemClicked()
        {
            GeneralSoundManager.Instance.GenericClick();
            if (attachedItem != null)
                Inventory.Instance.RemoveItem(attachedItem);
        }
    }
}