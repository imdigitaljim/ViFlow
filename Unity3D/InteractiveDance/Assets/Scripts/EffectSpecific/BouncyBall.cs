using UnityEngine;
using System.Collections;

public class BouncyBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    /**When a collision with the player object happens, "add force" to the ball so that it bounces off the player*/
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            //condition ? first_expression : second_expression;
            //The condition must evaluate to true or false. If condition is true, first_expression is evaluated and becomes the result.
            //If condition is false, second_expression is evaluated and becomes the result. Only one of the two expressions is evaluated.

            var rb = gameObject.GetComponent<Rigidbody>(); //the ball

            //if the player's postion is < the ball's postition, set to 1, else set to -1
            //This is setting the direction of the force to be applied
            var x = c.gameObject.transform.position.x < transform.position.x ? 1 : -1;
            var y = c.gameObject.transform.position.y < transform.position.y ? 1 : -1;
            rb.AddForce(new Vector3(x * 300, y * 300,0));
        }
    }
}
