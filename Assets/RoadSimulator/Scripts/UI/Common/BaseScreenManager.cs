using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Common
{
    public abstract class BaseScreenManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] protected SafeArea safeArea;

        private readonly Stack<GameObject> _currentPopups = new();
        private readonly Stack<GameObject> _currentPanels = new();
     
        public void ClosePopup()
        {
            var topmostPopup = _currentPopups.Pop();
            if (topmostPopup == null)
            {
                return;
            }

            var topmostPanel = _currentPanels.Pop();
            if (topmostPanel != null)
            {
                StartCoroutine(FadeOut(topmostPanel.GetComponent<Image>(), 0.2f, () => Destroy(topmostPanel)));
            }
        }

        protected void OpenPopup<T>(string popupName, object args = null, Action<T> onOpened = null) where T : BasePopup
        {
            StartCoroutine(OpenPopupAsync(popupName, args, onOpened));
        }

        private IEnumerator OpenPopupAsync<T>(string popupName, object args, Action<T> onOpened) where T : BasePopup
        {
            var request = Resources.LoadAsync<GameObject>(popupName);
            while (!request.isDone)
            {
                yield return null;
            }

            ShowPopup(request, args, onOpened);
        }

        private void ShowPopup<T>(ResourceRequest request, object args, Action<T> onOpened) where T : BasePopup
        {
            DrawPopupPanel();
            var popup = CreatePopup(request, args);

            onOpened?.Invoke(popup.GetComponent<T>());
            _currentPopups.Push(popup);
        }

        private void DrawPopupPanel()
        {
            var panel = new GameObject("Panel");
            var panelImage = panel.AddComponent<Image>();
            var panelCanvasGroup = panel.AddComponent<CanvasGroup>();
            var panelTransform = panel.GetComponent<RectTransform>();

            var color = new Color(149 / 255.0f, 149 / 255.0f, 149 / 255.0f);
            color.a = 0;
            panelImage.color = color;

            panelTransform.anchorMin = new Vector2(0, 0);
            panelTransform.anchorMax = new Vector2(1, 1);
            panelTransform.pivot = new Vector2(0.5f, 0.5f);
            panel.transform.SetParent(canvas.transform, false);
            panelCanvasGroup.blocksRaycasts = true;
            _currentPanels.Push(panel);
            StartCoroutine(FadeIn(panelImage, 0.2f));
        }

        private GameObject CreatePopup(ResourceRequest request, object args)
        {
            var popupGameObject = Instantiate(request.asset, canvas.transform, false) as GameObject;
            Assert.IsNotNull(popupGameObject);
            var popupComponent = popupGameObject.GetComponent<BasePopup>();

            popupComponent.SetParentScreenManager(this);
            popupComponent.OnAttach();
            popupComponent.SetArguments(args);
            popupComponent.OnCreated();

            return popupGameObject;
        }

        private static IEnumerator FadeIn(Graphic image, float time)
        {
            var alpha = image.color.a;
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                var color = image.color;
                color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
                image.color = color;
                yield return null;
            }
        }

        private static IEnumerator FadeOut(Graphic image, float time, Action onComplete)
        {
            var alpha = image.color.a;
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                var color = image.color;
                color.a = Mathf.Lerp(alpha, 0, t);
                image.color = color;
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}