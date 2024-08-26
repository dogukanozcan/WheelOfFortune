using DG.Tweening;
using Naku.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace Naku.WheelOfFortune
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
    public class WheelOfFortune : MonoBehaviour
    {
        [SerializeField] private bool m_isDebugEnabled = false;

        [SerializeField] private List<ItemStack> m_items;
       
        [SerializeField] private WheelPiece m_wheelPiecePrefab;

        [SerializeField] private ColorPaletteSO m_colorPalette;

        [SerializeField] private Transform m_inventoryButtonTransform;
        [SerializeField] private float m_moveAnimateTime = 2.5f;

        [SerializeField] private CooldownTimer m_cooldown;

        private WheelSpinner m_wheelSpinner; 
        private readonly List<WheelPiece> m_wheelPieces = new();

        public float PieceSize => 1.0f / m_items.Count;
        public float PieceAngle => 360 * PieceSize;

        private void OnValidate()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                var item = m_items[i];
                if(item.StackCount <= 0)
                {
                    m_items[i] = new(item.ItemSO, 1);
                }
            }
        }

        private void Awake()
        {
            m_wheelSpinner = GetComponent<WheelSpinner>();
            GenerateWheel();
            m_cooldown.StartCooldown(() => m_wheelSpinner.CanSpin = true);
        }

        private void OnEnable()
        {
            m_wheelSpinner.OnSpinEnd += OnSpinEnd;
        }

        private void OnDisable()
        {
            m_wheelSpinner.OnSpinEnd -= OnSpinEnd;
        }

        
        private void OnSpinEnd()
        {
            //REWARD
            //Calculation to determine the reward won
            var spinAngle = transform.rotation.eulerAngles.z;
            //stopped angle to index
            int stoppedIndex = m_items.Count-(int)(spinAngle / PieceAngle) -1;
            var reward = m_items[stoppedIndex];
            Debug.Log("Reward: <color=Yellow>"+ reward.ItemSO.ItemName + "</color>");

            //Highlight Animate
            var rewardPiece = m_wheelPieces[stoppedIndex];
            var highlightDuration = rewardPiece.HighlightAnimate();
            var moveDuration = RewardMoveAnimate(rewardPiece);

            //Stop spin until animation end
            m_wheelSpinner.CanSpin = false;
            if (m_isDebugEnabled)
            {
                MonoHelper.Delay(() => m_wheelSpinner.CanSpin = true, highlightDuration);
            }
            else
            {
                m_cooldown.ResetCooldown(() => m_wheelSpinner.CanSpin = true);
            }

            Inventory.Instance.AddItem(reward);
        }

        //Instantiate pieces, set rotation and attach item to piece
        public void GenerateWheel()
        {
            if (m_items.Count == 0) return;

            for (int i = 0; i < m_items.Count; i++)
            {
                var piece = Instantiate(m_wheelPiecePrefab, transform);
                piece.SetBackground(m_colorPalette.GetNextColor(), PieceSize);
                piece.AttachItem(m_items[i]);

                // Rotate pieces
                // for example; items.count is 5 || pieceSize will be 1/5 = 0.2 || for first item angle will be 360*0.2 = 72 degrees
                float nextAngle = (PieceAngle*(i + 1))+180f;
                piece.transform.Rotate(Vector3.forward * nextAngle);
                m_wheelPieces.Add(piece);
            }

        }

        public float RewardMoveAnimate(WheelPiece rewardPiece)
        {
            if (m_inventoryButtonTransform == null)
            {
                Debug.LogWarning("m_inventoryButtonTransform is null || RewardMoveAnimate will not play");
                return 0;
            }
              

            
            var animateReward = Instantiate(rewardPiece.ItemArea,transform.parent);

            animateReward.transform.SetPositionAndRotation(
                rewardPiece.ItemArea.transform.position, 
                rewardPiece.ItemArea.transform.rotation);

            animateReward.DOMove(m_inventoryButtonTransform.position, m_moveAnimateTime).SetEase(Ease.InOutBack);
            animateReward.DOScale(Vector3.zero, m_moveAnimateTime/2f).SetDelay(m_moveAnimateTime/2f);
            Destroy(animateReward.gameObject,m_moveAnimateTime+.1f);
            return m_moveAnimateTime;
        }
    }
}