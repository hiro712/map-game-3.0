using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Web3Hackathon
{
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";
            var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
            return obj.array != null ? new List<T>(obj.array) : new List<T>();
        }

        public static string ToJson<T>(IEnumerable<T> collection)
        {
            string json = JsonUtility.ToJson(new DummyNode<T>(collection));
            int start = DummyNode<T>.ROOT_NAME.Length + 4;
            int len = json.Length - start - 1;
            return json.Substring(start, len);
        }

        [Serializable]
        private struct DummyNode<T>
        {
            public const string ROOT_NAME = nameof(array);
            public T[] array;

            public DummyNode(IEnumerable<T> collection)
            {
                this.array = collection.ToArray();
            }
        }
    }
}