/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using System.Collections;

 // required for Coroutines



/// <summary>
/// Fades the screen from black after a new scene is loaded.
/// </summary>
public class ScreenFade : MonoBehaviour
{
	private enum ScreenState
	{
		VISIBLE,
		FADING,
		FADED
	}

	/// <summary>
	/// The initial screen color.
	/// </summary>
	public Color m_fadeColor = new Color (0.01f, 0.01f, 0.01f, 1.0f);

	private Material m_fadeMaterial = null;
	private ScreenState m_state = ScreenState.VISIBLE;

	private YieldInstruction m_fadeInstruction = new WaitForEndOfFrame ();

	/// <summary>
	/// Initialize.
	/// </summary>
	void Awake ()
	{
		// create the fade material
		m_fadeMaterial = new Material (Shader.Find ("Oculus/Unlit Transparent Color"));
	}

	public void FadeIn (float p_fadeTime)
	{
		StopAllCoroutines ();

		StartCoroutine (FadeCoroutine (p_fadeTime, true));
	}

	public void FadeOut (float p_fadeTime)
	{
		StopAllCoroutines ();

		StartCoroutine (FadeCoroutine (p_fadeTime, false));
	}

	/// <summary>
	/// Cleans up the fade material
	/// </summary>
	void OnDestroy ()
	{
		if (m_fadeMaterial != null)
		{
			Destroy (m_fadeMaterial);
		}
	}

	/// <summary>
	/// Fades alpha from 1.0 to 0.0
	/// </summary>
	IEnumerator FadeCoroutine (float p_fadeTime, bool p_fadeIn)
	{
		m_state = ScreenState.FADING;

		float elapsedTime = 0.0f;
		Color color = m_fadeColor;
		float alphaDelta = 1.0f / p_fadeTime;

		if (p_fadeIn == true)
		{
			color.a = 1.0f;
			alphaDelta *= -1.0f;
		} else
		{
			color.a = 0.0f;
		}

		m_fadeMaterial.color = color;

		while (elapsedTime < p_fadeTime)
		{
			yield return m_fadeInstruction;

			m_fadeMaterial.color = color;
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Clamp01 (color.a + Time.deltaTime * alphaDelta);
		};
			
		m_state = ((p_fadeIn == true) ? ScreenState.VISIBLE : ScreenState.FADED);
	}

	/// <summary>
	/// Renders the fade overlay when attached to a camera object
	/// </summary>
	void OnPostRender ()
	{
		if (m_state != ScreenState.VISIBLE)
		{
			m_fadeMaterial.SetPass (0);
			GL.PushMatrix ();
			GL.LoadOrtho ();
			GL.Color (m_fadeMaterial.color);
			GL.Begin (GL.QUADS);
			GL.Vertex3 (0f, 0f, -12f);
			GL.Vertex3 (0f, 1f, -12f);
			GL.Vertex3 (1f, 1f, -12f);
			GL.Vertex3 (1f, 0f, -12f);
			GL.End ();
			GL.PopMatrix ();
		}
	}
}
