using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Sense.BehaviourTree {
    public class BoolEvent : UnityEvent<bool> {

    }

    public class BoolValue : MonoBehaviour {
        [SerializeField]
        protected bool boolValue = false;

        virtual public bool Value {
            get {
                return boolValue;
            }
            set {
                boolValue = value;
                Changed.Invoke(value);
            }
        }

        [SerializeField]
        protected BoolEvent changed;

        virtual public BoolEvent Changed {
            get {
                return changed;
            }
        }
    }
}