using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MonoPoolable : MonoBehaviour
    {
        public virtual void OnGenerated()
        {
            this.gameObject.SetActive(false);
        }
        public virtual void OnSpawned()
        {
            this.gameObject.SetActive(true);
        }
        public virtual void OnReturned()
        {
            this.gameObject.SetActive(false);
        }
        public virtual void OnDestroyed() {}
    }
}
