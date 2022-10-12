using UnityEngine;
using System.Collections;
using System;

public class PlayerGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // singleton instance of the class
    private static PlayerGestureListener instance = null;

    // whether the needed gesture has been detected or not
    private bool raiseLeftHand;
    private bool raiseRightHand;
    private bool swipeDown;
    private bool wave;

    private bool handsUp;
    private bool swimming;

    /// <summary>
    /// Gets the singleton PlayerGestureListener instance.
    /// </summary>
    /// <value>The PlayerGestureListener instance.</value>
    public static PlayerGestureListener Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// Determines whether raise left hand is detected.
    /// </summary>
    /// <returns><c>true</c> if raise left hand is detected; otherwise, <c>false</c>.</returns>
    public bool isRaiseLeftHand()
    {
        if (raiseLeftHand)
        {
            raiseLeftHand = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether raise right hand is detected.
    /// </summary>
    /// <returns><c>true</c> if raise right hand is detected; otherwise, <c>false</c>.</returns>
    public bool isRaiseRightHand()
    {
        if (raiseRightHand)
        {
            raiseRightHand = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether swipe down is detected.
    /// </summary>
    /// <returns><c>true</c> if swipe down is detected; otherwise, <c>false</c>.</returns>
    public bool IsSwipeDown()
    {
        if (swipeDown)
        {
            swipeDown = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether wave is detected.
    /// </summary>
    /// <returns><c>true</c> if wave is detected; otherwise, <c>false</c>.</returns>
    public bool IsWave()
    {
        if (wave)
        {
            wave = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether handsUp is detected.
    /// </summary>
    /// <returns><c>true</c> if handsUp is detected; otherwise, <c>false</c>.</returns>
    public bool IsHandsUp()
    {
        if (handsUp)
        {
            handsUp = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether swimming is detected.
    /// </summary>
    /// <returns><c>true</c> if swimming is detected; otherwise, <c>false</c>.</returns>
    public bool IsSwimming()
    {
        if (swimming)
        {
            swimming = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserDetected(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
        manager.DetectGesture(userId, KinectGestures.Gestures.Wave);

        manager.DetectGesture(userId, KinectGestures.Gestures.HandsUp);
        manager.DetectGesture(userId, KinectGestures.Gestures.Swimming);
    }

    /// <summary>
    /// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserLost(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;
    }

    /// <summary>
    /// Invoked when a gesture is in progress.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="progress">Gesture progress [0..1]</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;
    }

    /// <summary>
    /// Invoked if a gesture is completed.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;

        //string sGestureText = gesture + " detected";
        //Debug.Log(sGestureText);

        if (gesture == KinectGestures.Gestures.RaiseLeftHand)
            raiseLeftHand = true;
        else if (gesture == KinectGestures.Gestures.RaiseRightHand)
            raiseRightHand = true;
        else if (gesture == KinectGestures.Gestures.SwipeDown)
            swipeDown = true;
        else if (gesture == KinectGestures.Gestures.Wave)
            wave = true;

        else if (gesture == KinectGestures.Gestures.HandsUp)
            handsUp = true;
        else if (gesture == KinectGestures.Gestures.Swimming)
            swimming = true;

        return true;
    }

    /// <summary>
    /// Invoked if a gesture is cancelled.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;

        return true;
    }

    void Awake()
    {
        instance = this;
    }

    public void StopAllGestures()
    {
        raiseLeftHand = false;
        raiseRightHand = false;
        swipeDown = false;
        wave = false;
        handsUp = false;
        swimming = false;
    }
}
