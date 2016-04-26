#pragma strict

public var pA:GameObject;
public var pB:GameObject;


function Start () {
		var between:Vector3 = pB.transform.position - pA.transform.position;
		var distance:float = between.magnitude;
		transform.localScale.z = distance;
		transform.position = pA.transform.position + ( between/2.0);
		transform.LookAt(pB.transform.position);
}

function Update () {
		var between:Vector3 = pB.transform.position - pA.transform.position;
		var distance:float = between.magnitude;
		transform.localScale.z = distance;
		transform.position = pA.transform.position + ( between/2.0);
		transform.LookAt(pB.transform.position);
}