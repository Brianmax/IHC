using System;
using System.Collections;
using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class ExampleInteractiveItem : MonoBehaviour
    {
        [SerializeField] private Material m_NormalMaterial;
        [SerializeField] private Material m_OverMaterial;
        [SerializeField] private Material m_ClickedMaterial;
        [SerializeField] private Material m_DoubleClickedMaterial;
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private Renderer m_Renderer;

        private Coroutine timeCounterRoutine;  // Reference to the coroutine that controls the time counter, used to stop it if required.        

        private bool gazeOver = false; // Whether the user is currently looking at the interactive item.
        private float timer = 0;

        private void Awake()
        {
            m_Renderer.material = m_NormalMaterial;
        }

        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
            m_InteractiveItem.OnDown += HandleDown;
            m_InteractiveItem.OnUp += HandleUp;
        }

        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
            m_InteractiveItem.OnDown -= HandleDown;
            m_InteractiveItem.OnUp -= HandleUp;
        }

        private void OnUpdate()
        {

        }

        private IEnumerator Count()
        {
            // When the bar starts to fill, reset the timer.
            timer = 0.0f;

            // The amount of time it takes to fill is either the duration set in the inspector, or the duration of the radial.
            float fillTime = 2;

            // Until the timer is greater than the fill time...
            while (timer < fillTime)
            {
                // ... add to the timer the difference between frames.
                timer += Time.deltaTime;

                // Wait until next frame.
                yield return null;

                // If the user is still looking at the bar, go on to the next iteration of the loop.
                if (gazeOver)
                    continue;

                // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
                timer = 0f;
                yield break;
            }

            GameObject.Find("MainCamera").GetComponent<ScreenFade>().FadeOut(2.0f);
        }

        //Handle the Over event
        private void HandleOver()
        {
            m_Renderer.material = m_OverMaterial;

            // The user is now looking at the interactive item.
            gazeOver = true;
        }

        //Handle the Out event
        private void HandleOut()
        {
            m_Renderer.material = m_NormalMaterial;

            // The user is no longer looking at the interactive item.
            gazeOver = false;

            // Reset the timer
            timer = 0.0f;
        }

        //Handle the Click event
        private void HandleClick()
        {
            m_Renderer.material = m_ClickedMaterial;
        }

        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
            m_Renderer.material = m_DoubleClickedMaterial;
        }

        //Handle the Down event
        private void HandleDown()
        {
            if (gazeOver)
                timeCounterRoutine = StartCoroutine(Count());
        }

        //Handle the Up event
        private void HandleUp()
        {
            // Reset the timer
            timer = 0.0f;
        }
    }
}