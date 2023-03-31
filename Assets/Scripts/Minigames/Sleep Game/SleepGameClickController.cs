using System.Collections;
using UnityEngine;

public class SleepGameClickController : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckClickLocation();
    }

    private void CheckClickLocation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                SleepGameDreamController dreamController = hit.collider.gameObject.GetComponent<SleepGameDreamController>();

                if (dreamController != null)
                    dreamController.DestroyItem();
            }
        }
    }
}
