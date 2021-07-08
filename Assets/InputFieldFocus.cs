using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldFocus : InputField
{
    IEnumerator Nextframe_MoveTextEnd()
    {
        yield return null;
   
        MoveTextEnd(false);
    }

    /*public override void OnDeselect(BaseEventData eventData)
    {       
        ActivateInputField();       
        StartCoroutine(Nextframe_MoveTextEnd());
       
        base.OnDeselect(eventData);
    }*/
    
    public override void OnSelect(BaseEventData eventData)
    {   
        ActivateInputField();
        StartCoroutine(Nextframe_MoveTextEnd());
       
        base.OnSelect(eventData);
    }
}
