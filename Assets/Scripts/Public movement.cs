using UnityEngine;
using System.Collections;
public class Publicmovement : MonoBehaviour
{
    [SerializeField] public float speed = 50f;

    
    [SerializeField] public Vector2 direction = Vector2.up;
    public Vector2 positionInitiale;
   

   
    void Update ()
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
    
}