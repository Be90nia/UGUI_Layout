using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LayoutIgnorer : UIBehaviour, ILayoutIgnorer
{
    private bool _ignoreLayout = true;

    public bool ignoreLayout
    {
        get { return _ignoreLayout; }
    }
}

