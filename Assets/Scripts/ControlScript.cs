using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ControlScript : MonoBehaviour,IDragHandler, IPointerClickHandler
{   [SerializeField]
    GameObject legGun;

    [SerializeField]
    GameObject headGun;

    [SerializeField]
    float sensivityX,sensivityY;

    [SerializeField]
    LineRender lineRender;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    GameObject placeSpawn;
    
    const float MINVALUEY = 24;
    const float MAXVALUEY = 24;

   
    private float x, y;
  
    int count;

    public void OnDrag(PointerEventData eventData)
    {
        x = eventData.delta.x / 100;
        y = eventData.delta.y / 100;

        var legRotation = legGun.transform.eulerAngles;
        var headRotation = headGun.transform.eulerAngles;
        ////var rotation = camera.transform.eulerAngles;
        //rotation.y += x*sensivityY;
        //rotation.x += -y*sensivityX;
        legRotation.y += x * sensivityY;
        headRotation.z += y * sensivityX;
        headRotation.y += x * sensivityY;
        if (legRotation.y > 90 && legRotation.y<200 )
        {
            legRotation.y = 90;
            headRotation.y = 90;
        }
        if (legRotation.y < 270 && legRotation.y>90)
        {
            legRotation.y = 270;
            headRotation.y = 270;
        }



        legGun.transform.rotation = Quaternion.Euler(legRotation.x, legRotation.y, 0f);
        headGun.transform.rotation = Quaternion.Euler(0f, headRotation.y, headRotation.z);
        lineRender.renderTrajectory(placeSpawn.transform.position, placeSpawn.transform.forward*50f);

        //camera.transform.rotation = Quaternion.Euler(rotation.x,rotation.y,0f);










    }


    // Start is called before the first frame update
    void Start()
    {
        sensivityX = 14f;
        sensivityY = 14f;

    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shootBullet();
        lineRender.rendereTrajectoryClear();
    }

    void shootBullet()
    {
        var bulletCreaterd = Instantiate(bullet,placeSpawn.transform.position, placeSpawn.transform.rotation) as GameObject;
        bulletCreaterd.GetComponent<Rigidbody>().velocity = bulletCreaterd.transform.forward * 50f;
        Destroy(bulletCreaterd, 5f);

    }
}


