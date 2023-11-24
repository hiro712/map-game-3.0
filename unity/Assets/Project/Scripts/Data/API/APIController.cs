using System;
using System.Collections.Generic;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Project.Scripts.Data.NFT;
using UniRx;

namespace Web3Hackathon
{
    public class APIController : SingletonMonoBehaviour<APIController>
    {
        private ReactiveProperty<string> _baseUrl = new ReactiveProperty<string>("http://localhost:3000");
        public IReadOnlyReactiveProperty<string> BaseURL => _baseUrl;

        public void SetBaseURL(string url)
        {
            _baseUrl.Value = url;
            Debug.Log("SetBaseURL: " + _baseUrl);
        }

        public async UniTask<ResImagePath> GenerateImage(float lat, float lng)
        {
            string url = $"{_baseUrl}/api/generate";
            var requestBody = new ReqBodyLatLng()
            {
                lat = lat,
                lng = lng
            };
            string jsonBody = JsonUtility.ToJson(requestBody);
            return await PostRequest<ResImagePath>(url, jsonBody);
        }

        public async UniTask<List<ImageData>> UploadImageData(string address, float lat, float lng, int status)
        {
            string url = $"{_baseUrl}/api/manage";
            var requestBody = new ReqBodyImage()
            {
                address = address,
                status = status,
                lat = lat,
                lng = lng
            };
            var jsonBody = JsonUtility.ToJson(requestBody);
            return await PostRequestList<ImageData>(url, jsonBody);
        }

        public async UniTask<List<ImageData>> UpdateImageData(int id, string address, int status, string image)
        {
            string url = $"{_baseUrl}/api/manage";
            var requestBody = new ReqBodyUpdate()
            {
                id = id,
                address = address,
                status = status,
                image = image
            };
            var jsonBody = JsonUtility.ToJson(requestBody);
            return await PutRequestList<ImageData>(url, jsonBody);
        }

        public async UniTask<List<ImageData>> GetImageDataList(string address)
        {
            string url = $"{_baseUrl}/api/manage?address={address}";
            return await GetRequest<ImageData>(url);
        }

        // HTTP POSTリクエスト
        private async UniTask<List<T>> PostRequestList<T>(string url, string jsonBody) where T : new()
        {
            Debug.Log("PostRequest, url: " + url + ", jsonBody: " + jsonBody);
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    List<T> res = new List<T>();
                    return res;
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    List<T> res = JsonHelper.FromJson<T>(responseText);
                    return res;
                }
            }
        }

        private async UniTask<T> PostRequest<T>(string url, string jsonBody) where T : new()
        {
            Debug.Log("PostRequest, url: " + url + ", jsonBody: " + jsonBody);
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    T res = new T();
                    return res;
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    T res = JsonUtility.FromJson<T>(responseText);
                    return res;
                }
            }
        }

        // HTTP PUTリクエスト
        private async UniTask<List<T>> PutRequestList<T>(string url, string jsonBody) where T : new()
        {
            Debug.Log("PutRequest, url: " + url + ", jsonBody: " + jsonBody);
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    List<T> res = new List<T>();
                    return res;
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    List<T> res = JsonHelper.FromJson<T>(responseText);
                    return res;
                }
            }
        }

        // HTTP GETリクエスト
        // 帰ってくるあたいはImageDataの配列
        private async UniTask<List<T>> GetRequest<T>(string url) where T : new()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    List<T> res = new List<T>();
                    return res;
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    List<T> res = JsonHelper.FromJson<T>(responseText);
                    return res;
                }
            }
        }
    }
}