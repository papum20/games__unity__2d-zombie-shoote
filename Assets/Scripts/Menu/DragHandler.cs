using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragHandler : MonoBehaviour
{

    public GameObject player;
    public Image menuPanel;
    public Button exitControlModifier;

    Vector2 _screenPosition;
    Vector2 _worldPosition;
    bool clicking;



    void Start()
    {
        clicking = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {

            if (Input.GetMouseButton(0))
                _screenPosition = Input.mousePosition;
            else if (Input.touchCount > 0)
                _screenPosition = Input.GetTouch(0).position;


            _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

            if (clicking)
            {
                if (_worldPosition.y >= 1 && _worldPosition.y <= 8)
                {
                    transform.position = new Vector2(transform.position.x, _worldPosition.y);
                    float touchPercent = Camera.main.WorldToScreenPoint(transform.position).y / Camera.main.scaledPixelHeight;
                    player.GetComponent<PlayerMovement>().SetTouch(touchPercent);
                    player.GetComponent<Weapon>().SetTouch(touchPercent);
                    SaveSystem.SaveOptions(0, 0, 0, -1, touchPercent);
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(_worldPosition, Vector2.zero);
                if (hitInfo.collider != null && hitInfo.transform.tag == "draggable")
                {
                    if (transform.position.y >= 1 && transform.position.y <= 8)
                    {
                        transform.position = new Vector2(transform.position.x, _worldPosition.y);
                        float touchPercent = Camera.main.WorldToScreenPoint(transform.position).y / Camera.main.scaledPixelHeight;
                        player.GetComponent<PlayerMovement>().SetTouch(touchPercent);
                        player.GetComponent<Weapon>().SetTouch(touchPercent);
                        SaveSystem.SaveOptions(0, 0, 0, -1, touchPercent);
                    }                   
                    clicking = true;
                }
            }

        }

        else
            clicking = false;

    }



    

}
