using System;
using UnityEngine;

namespace Common
{
    public abstract class SingletonMonoBehaviour<T>: MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Type t = typeof(T);
                    _instance = (T)FindObjectOfType(t);
                    if (_instance == null)
                    {
                        Debug.LogError(t + " is nothing");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (this != Instance)
            {
                Destroy(this);
                Debug.LogError(
                    typeof(T) +
                    " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                    " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
                return;
            }
        }
    }
}