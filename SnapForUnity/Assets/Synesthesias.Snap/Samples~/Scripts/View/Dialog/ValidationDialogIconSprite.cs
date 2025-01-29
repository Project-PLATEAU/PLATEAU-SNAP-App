using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    [Serializable]
    public class ValidationDialogIconSprite
    {
        [SerializeField] private DialogIconDefine icon;
        [SerializeField] private Sprite sprite;

        /// <summary>
        /// アイコンの定義
        /// </summary>
        public DialogIconDefine Icon
            => icon;

        /// <summary>
        /// スプライト
        /// </summary>
        public Sprite Sprite
            => sprite;
    }
}