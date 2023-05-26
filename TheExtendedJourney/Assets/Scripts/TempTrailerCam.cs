using UnityEngine;

public class TempTrailerCam : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] GameObject[] otherCams;

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            ui.SetActive(false);
            foreach (GameObject obj in otherCams)
            {
                obj.SetActive(false);
            }
        }
        */
    }
}