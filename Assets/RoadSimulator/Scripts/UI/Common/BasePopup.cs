using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RoadSimulator.Scripts.UI.Common
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField] private UnityEvent onClose;
        [SerializeField] private UnityEvent onOpen;

        protected BaseScreenManager Context;

        private Animator _animator;

        private object _args;

        public void SetArguments(object args)
        {
            _args = args;
        }

        public void SetParentScreenManager(BaseScreenManager parentScreenManager)
        {
            if (Context != null)
            {
                throw new Exception("Popup already has Context");
            }

            Context = parentScreenManager;
        }

        protected object GetArguments()
        {
            return _args;
        }

        public virtual void OnAttach()
        {
        }

        public virtual void OnCreated()
        {
        }

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            onOpen?.Invoke();
        }

        protected void Close()
        {
            onClose?.Invoke();
            if (Context != null)
            {
                Context.ClosePopup();
            }

            if (_animator != null)
            {
                _animator.Play("Close");
                StartCoroutine(DestroyPopup());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator DestroyPopup()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}