using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;

namespace ChatClientExample
{
    public class ChatCanvas : MonoBehaviour
    {
        public static Color chatColor = new Color(.85f, .85f, .85f);
        public static Color joinColor = new Color(.25f, .95f, .25f);
        public static Color leaveColor = new Color(.95f, .25f, .25f);

        public GameObject textPrefab;
        public Transform chatPanel;

        public Queue<GameObject> textInstances = new Queue<GameObject>();

        public int maxMessages = 32;

        public void NewMessage(string message, Color color) {
            GameObject newInstance = GameObject.Instantiate(textPrefab);

            newInstance.GetComponent<Text>().text = $"{message}";
            newInstance.GetComponent<Text>().color = color;
            newInstance.transform.SetParent( chatPanel );

            newInstance.SetActive(true);

            if (textInstances.Count > maxMessages) {
                Destroy(textInstances.Dequeue());
            }

            textInstances.Enqueue(newInstance);
        }
    }
}