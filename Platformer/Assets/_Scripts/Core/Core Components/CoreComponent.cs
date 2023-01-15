using UnityEngine;

namespace Gamecells.CoreSystem
{
    public class CoreComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core;

        protected virtual void Awake()
        {
            if (transform.parent.TryGetComponent(out core))
                core.AddComponent(this);
            else
                Debug.LogError("There is no Core on the parent!");
        }

        public virtual void LogicUpdate() { }
    }
}