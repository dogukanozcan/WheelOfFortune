using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Naku.WheelOfFortune
{
    public class WheelPiece : MonoBehaviour
    {
        [SerializeField] private Image m_background;
        [SerializeField] private Image m_itemImage;
        [SerializeField] private TextMeshProUGUI m_itemNameLabel;
        [SerializeField] private TextMeshProUGUI m_itemCountLabel;
        public RectTransform ItemArea;

        public void SetBackground(Color color, float fillAmount)
        {
            m_background.color = color;
            m_background.fillAmount = fillAmount;
            SetItemAreaRot();
        }
        //Assgin item to piece
        public void AttachItem(ItemStack itemStack)
        {
            m_itemImage.sprite = itemStack.ItemSO.ItemSprite;
            m_itemNameLabel.text = itemStack.ItemSO.ItemName;
            if(itemStack.StackCount>1) m_itemCountLabel.text = itemStack.StackCount.ToString();
        }

        public void SetItemAreaRot()
        {
            #region Variables
            var fillAmount = m_background.fillAmount; //Get fillAmount
            var scaling = Mathf.Lerp(.3f, 1f, fillAmount / 0.2f); //Calculate scaling ratio
            float pieceAngle = 360f * fillAmount; //Find piece angle
            float halfAngle = pieceAngle / 2f; //To find the center, we need half of the angle of the piece.
            #endregion

            ItemArea.RotateAround(transform.position, Vector3.forward, -halfAngle);
            ItemArea.localScale = Vector3.one * scaling;
        }

        public float HighlightAnimate()
        {
            var animationDuration = .8f;
            var tempColor = m_background.color;
            var whiterColor = Color.Lerp(tempColor, Color.white, .65f);
            m_background.DOBlendableColor(whiterColor, animationDuration / 2f)
                .OnComplete(() =>
                {
                    m_background.DOBlendableColor(tempColor, animationDuration / 2f);
                });

            transform.DOPunchScale(Vector3.one * 0.12f, animationDuration);

            return animationDuration;
        }

        #region Obsolete Centered
        [Obsolete]
        public void SetImageAndTitleCentered()
        {
            #region Variables
            var fillAmount = m_background.fillAmount; //Get fillAmount
            var scaling = Mathf.Lerp(.1f, 1f, fillAmount / 0.2f); //Calculate scaling ratio
            float pieceAngle = 360f * fillAmount; //Find piece angle
            float halfAngle = pieceAngle / 2f; //To find the center, we need half of the angle of the piece.


            #endregion
            #region Name Label
            float labelPositionY = Mathf.Abs(m_itemNameLabel.transform.localPosition.y); //Get default position
            var labelPositionX = GetOppositeLength(halfAngle, labelPositionY); //Trigonometric function

            if (halfAngle >= 90) labelPositionY = 0;
            var labelCenteredPos = new Vector2(-labelPositionX, -labelPositionY);
            var labelCenteredRot = Quaternion.Euler(Vector3.forward * (180 - halfAngle));

            m_itemNameLabel.transform.SetLocalPositionAndRotation(labelCenteredPos, labelCenteredRot); //nameLabel centered
            m_itemNameLabel.transform.localScale = Vector3.one * scaling;
            #endregion
            #region Item Image

            float imagePositionY = Mathf.Abs(m_itemImage.transform.localPosition.y); //Get default position
            var imagePositionX = GetOppositeLength(halfAngle, imagePositionY); //Trigonometric function

            if (halfAngle >= 90) imagePositionY = 0;
            var imageCenteredPos = new Vector2(-imagePositionX, -imagePositionY);
            var imageCenteredRot = Quaternion.Euler(Vector3.forward * (180 - halfAngle));

            m_itemImage.transform.SetLocalPositionAndRotation(imageCenteredPos, imageCenteredRot); //nameLabel centered
            m_itemImage.transform.localScale = Vector3.one * scaling;
            #endregion
        }

        //tan(x) = Opposite/Adjacent ; Opposite = tan(x)*Adjacent
        [Obsolete]
        public float GetOppositeLength(float angle, float adjacentLength)
        {
            if (angle >= 90)
                return adjacentLength;

            return adjacentLength * Mathf.Tan(angle * Mathf.Deg2Rad);
        }
        #endregion
    }
}