using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;
using System;

public class GrabbingController : MonoBehaviour
{
    public bool isGrabbing;
    public bool isDetaching;
    // private bool targetReached {get { return TargetReached(currentTask); } }
    private EnvironmentController environmentController;
    private CollisionController collisionController;
    private CollisionDetector CollisionDetector;
    private TaskFileTemplate currentTask;
    private GameObject grabbingObject;
    private GameObject grabbingTarget;
    private GameObject tool;
    public SphereCollider grabbingSphereCollider;
    private Rigidbody rigidBody;

    public void Init(CurriculumFileTemplate curriculumConfiguration)
    {   
        isGrabbing = false;
        isDetaching = false;
        grabbingObject = GameObject.Find(curriculumConfiguration.grabbingObject);
        rigidBody = grabbingObject.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        grabbingObject.transform.SetParent(null);
     }

    public void ApplyGrabbing(ActionSegment<float> actions, CollisionController collisionController,
    CollisionDetector collisionDetector, bool grabbingCollide, float contactAngle, float maximumContactAngle)
    {
        float grabbing = actions[4];        
        if((grabbing >= -0.8f) && !isGrabbing) {GrabOn(grabbingCollide, contactAngle, maximumContactAngle);}
        else if(grabbing < -0.8f && isGrabbing) {Graboff();}     
    }
    public void GrabOn(bool grabbingCollide, float contactAngle, float maximumContactAngle)
    {
        if (grabbingCollide && contactAngle <= maximumContactAngle)
        {
        tool = GameObject.Find("RefObject");
        grabbingObject.transform.SetParent(tool.transform);
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        isGrabbing = true;
        isDetaching = false;
        Debug.Log("Grabbing on");
        }
    }
    public void Graboff()
    {
        isGrabbing = false;
        isDetaching = true;
        grabbingObject.transform.SetParent(null);
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.mass = 60f;
        Debug.Log("Grabbing off");
    }
    public void ResetGrabbingController()
    {
        isGrabbing = false;
        isDetaching = false;
        grabbingObject.transform.SetParent(null);
        rigidBody.isKinematic = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        rigidBody.useGravity = true;
    }
  
}
