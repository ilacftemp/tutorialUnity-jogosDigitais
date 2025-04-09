using UnityEngine;

public class RestartMenuController : MonoBehaviour
{
    public GameObject restartMenu;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isOpen = !isOpen;
            restartMenu.SetActive(isOpen);

            Time.timeScale = isOpen ? 0f : 1f;
        }
    }
}