using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szn.Framework.Ui
{
    public class UiBase : MonoBehaviour
    {
        #region Event

        #endregion

        private Animator anim;

        private Transform trans;

        protected UiController UiCtrl;

        public GameObject RootGameObj { get; private set; }
        public RectTransform RootRectTrans { get; private set; }

        public UiData Data { get; private set; }

        public void Init(UiData InData, UiController InUiController)
        {
            trans = transform;
            RootGameObj = trans.Find(UiConfig.UI_ROOT_GAME_OBJECT_NAME).gameObject;
            RootRectTrans = RootGameObj.GetComponent<RectTransform>();

            Data = InData;
            UiCtrl = InUiController;

            if (!string.IsNullOrEmpty(Data.BackgroundSprite))
            {
                Transform bgTrans = trans.Find(UiConfig.UI_BACKGROUND_GAME_OBJECT_NAME);
                if (null != bgTrans)
                {
                    Image bgImg = bgTrans.GetComponent<Image>();
                    bgImg.sprite = ResourceController.Instance.LoadSprite(Data.BackgroundSprite);

                    Button bgBtn = bgTrans.GetComponent<Button>();
                    bgBtn.onClick.AddListener(() => UiCtrl.CloseUI(Data.Name));
                }
            }
        }
    }
}