Shader "Custom/Blend2Textures"
{
	Properties
	{
		[HideInInspector] _Blend("Blend", Range(0, 1)) = 0
		[HideInInspector] _CurrentTexture("Current Texture", 2D) = ""
		[HideInInspector] _NextTexture("Next Texture", 2D) = ""
	}

	SubShader
	{
		Pass
		{
			SetTexture[_CurrentTexture]
			SetTexture[_NextTexture]
			{
				ConstantColor(0,0,0,[_Blend])
				Combine texture Lerp(constant) previous
			}
		}
	}
}
