using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class ActionInvokedPredicate : IPredicate
{
    bool hasBeenInvoked;

    public ActionInvokedPredicate(ref Action action)
    {
        action += OnInvoke;
    }
    public ActionInvokedPredicate(ref UnityEvent ev)
    {
        ev.AddListener(OnInvoke);
    }

    private void OnInvoke()
    {
        hasBeenInvoked = true;
    }

    public bool Evaluate()
    {
        bool returnValue = hasBeenInvoked;

        hasBeenInvoked = false;

        return returnValue;
    }
}

