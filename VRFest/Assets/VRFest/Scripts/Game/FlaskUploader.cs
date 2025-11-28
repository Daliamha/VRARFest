// FlaskUploader.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

public class FlaskUploader : MonoBehaviour
{
    // URL вашего Flask-сервера
    private const string UPLOAD_URL = "http://127.0.0.1:5000/upload";

    /// <summary>
    /// Отправляет список игроков на сервер
    /// </summary>
    public void SendPlayersToServer(List<Player> players)
    {
        StartCoroutine(SendRequest(players));
    }

    private System.Collections.IEnumerator SendRequest(List<Player> players)
    {
        // Преобразуем List<Player> в массив — Unity так требует для JsonUtility
        Player[] playerArray = players.ToArray();

        // Сериализуем в JSON
        string json = JsonConvert.SerializeObject(playerArray);
        Debug.Log("Отправляется JSON: " + json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(UPLOAD_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Успешно отправлено! Ответ сервера: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("❌ Ошибка отправки: " + request.error);
            }
        }
    }
}