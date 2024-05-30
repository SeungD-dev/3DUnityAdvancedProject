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

            /* �̺�Ʈ ���� Ű�� �����ϴ��� �˻�. �����ϸ� ����Ʈ�� �߰� */
            if (Listeners.TryGetValue(eventType, out ListenList))
            {
                ListenList.Add(Listener);
                return;
            }

            /* ������ ���ο� ����Ʈ ���� */
            ListenList = new List<IListener>();
            ListenList.Add(Listener);
            Listeners.Add(eventType, ListenList);    /* ������ ����Ʈ�� �߰� */
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
