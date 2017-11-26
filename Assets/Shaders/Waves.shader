﻿Shader "II/Waves" 
{
	Properties
	{
		_Color("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Diffuse Texture", 2D) = "white" {}
		_SpecColor("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Float) = 10
		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0
	}

	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float _Shininess;
			uniform float4 _RimColor;
			uniform float _RimPower;

			uniform float4 _LightColor0;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			vertexOutput vert(vertexInput v)
			{
				//float4 v0 = mul(unity_ObjectToWorld, v.vertex);
				//v0.y = (sin(v0.x * 1.0 + _Time[1] * 1.0) + sin(v0.x * 2.3 + _Time[1] * 1.5) + sin(v0.x * 3.3 + _Time[1] * 0.4)) / 3.0;
				//v.vertex = mul(unity_WorldToObject, v0);

				vertexOutput o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize( mul(float4(v.normal, 0.0f), unity_WorldToObject).xyz );	
				o.tex = v.texcoord;

				return o;	
			}

			float4 frag(vertexOutput i) : COLOR
			{
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 normalDirection = i.normalDir;
				float3 lightDirection;
				float atten;

				if (_WorldSpaceLightPos0.w == 0.0)
				{
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float dis = length(fragmentToLightSource);
					atten = 1.0f / dis;
					lightDirection = normalize(fragmentToLightSource);
				}

				float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
				float3 specularReflection = _SpecColor.rgb * saturate(dot(normalDirection, lightDirection)) *
				pow( saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

				float rim = 1.0f - saturate( dot(viewDirection, normalDirection) );
				float3 rimLight = saturate(_LightColor0.xyz * _RimColor.xyz * 
					dot(normalDirection, lightDirection) * 
					pow(rim, _RimPower));

				float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.xyz + rimLight;

				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);

				return float4(lightFinal * _Color.xyz * tex.xyz, 1.0);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
