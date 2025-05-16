using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker.AM
{
    public interface IObserver<T>
    {
        void OnStateChanged(T newState);
    }

    public abstract class UIStateObserver<T> : MonoBehaviour
    {
        private List<IObserver<T>> observers = new List<IObserver<T>>();
        private T currentState;

        // State property
        public T CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                Debug.Log($"{currentState} received");
                NotifyObservers();
            }
        }

        // Register an observer
        public void RegisterObserver(IObserver<T> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                Debug.Log("Registered...");
            }
        }

        // Unregister an observer
        public void UnregisterObserver(IObserver<T> observer)
        {
            observers.Remove(observer);
            Debug.Log("Unregistered...");
        }

        // Notify all observers of a state change
        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.OnStateChanged(currentState);
                Debug.Log("Notification Sending...");
            }
        }
    }
}
