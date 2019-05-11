using UnityEngine;

public class InputWrapperTappedOnBomb: IInputWrapper
{


    public bool IsTapped(GameObject gameObject)
    {
        if (Input.touchCount <= 0 || GameManager.isGameOver)
        {
            return false;
        }
        
        Touch currentTouch = Input.touches[0];
        if (currentTouch.phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(currentTouch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Bomb") && raycastHit.collider.gameObject == gameObject)
                {

                    return true;
                }
                
            }
        }

        return false;
    }
}