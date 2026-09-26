using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class Publicmovement : MonoBehaviour
{
    [SerializeField] public float speed = 50f;
    private bool PublicHappy = false;
    [SerializeField] public Vector2 direction = Vector2.up;
    public Vector2 positionInitiale;
    private bool publicPresent = false;



    void Start()
    {


    }

    void Update()
    {
        if (PublicHappy == true)
        {

            if (publicPresent == true)
            {



                transform.Translate(direction * speed * Time.deltaTime);


                if (direction == Vector2.up && transform.position.y >= positionInitiale.y)
                {
                    direction = Vector2.down;

                }
                else if (direction == Vector2.down && transform.position.y <= positionInitiale.y - 1f)
                {
                    direction = Vector2.up;

                }
            }
          
            else if (publicPresent == false)
            {
                transform.position = positionInitiale;
                publicPresent = true;
            }

        }
        if (PublicHappy == false)
        {
            transform.position = positionInitiale - new Vector2(0, 1000f);
        }
    }
}