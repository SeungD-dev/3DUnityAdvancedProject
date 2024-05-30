using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EventManager : Singleton<EventManager>
    {
       private Dictionary<EventType, List<IListener>> Listeners =
            new Dictionary<EventType, List<IListener>>();

        public void AddListener(EventType eventType, IListener Listener)
        {
            List<IListener> ListenList = null;

            /* 이벤트 형식 키가 존재하는지 검사. 존재하면 리스트에 추가 */
            if (Listeners.TryGetValue(eventType, out ListenList))
            {
                ListenList.Add(Listener);
                return;
            }

            /* 없으면 새로운 리스트 생성 */
            ListenList = new List<IListener>();
            ListenList.Add(Listener);
            Listeners.Add(eventType, ListenList);    /* 리스너 리스트에 추가 */
        }

        public void PostNotification(EventType eventType, Component Sender, object param = null)
        {
            List<IListener> ListenList = null;

            if (!Listeners.TryGetValue(eventType, out ListenList))
                return;


            for (int i = 0; i < ListenList.Count; i++)
                ListenList?[i].OnEvent(eventType, Sender, param);
        }

        public void RemoveEvent(EventType eventType) => Listeners.Remove(eventType);

        public void RemoveRedundancies()
        {
            Dictionary<EventType, List<IListener>> newListeners =
                       new Dictionary<EventType, List<IListener>>();

            foreach (KeyValuePair<EventType, List<IListener>> Item in Listeners)
            {
                for (int i = Item.Value.Count - 1; i >= 0; i--)
                {
                    if (Item.Value[i].Equals(null))
                        Item.Value.RemoveAt(i);
                }

                if (Item.Value.Count > 0)
                    newListeners.Add(Item.Key, Item.Value);
            }

            Listeners = newListeners;
        }

        void OnLevelWasLoaded()
        {
            RemoveRedundancies();
        }
    }
}
