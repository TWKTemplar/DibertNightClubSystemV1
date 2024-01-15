Shader "Delta/Blackout"
{
    Properties
    {
        [Enum(Off,0,On,1)] _IsBlackedOut ("Is Blacked Out", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay+1000" "IgnoreProjector"="True" }
        LOD 100
        
        Cull Front
        ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 world : TEXCOORD0;
                float4 depthTextureGrabPos : TEXCOORD1;
                float4 rayFromCamera : TEXCOORD2;
                float3 cameraPos : TEXCOORD3;
            };

            #define UMP UNITY_MATRIX_P
            inline float4 CalculateObliqueFrustumCorrection()
			{
				float x1 = -UMP._31 / (UMP._11 * UMP._34);
				float x2 = -UMP._32 / (UMP._22 * UMP._34);
				return float4(x1, x2, 0, UMP._33 / UMP._34 + x1 * UMP._13 + x2 * UMP._23);
			}
			static float4 ObliqueFrustumCorrection = CalculateObliqueFrustumCorrection();
            inline float CorrectedLinearEyeDepth(float z, float correctionFactor)
			{
				return 1.f / (z / UMP._34 + correctionFactor);
			}
			#undef UMP
            
            UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
            float4 _CameraDepthTexture_TexelSize;

            bool _IsBlackedOut;
            float3 _Udon_MinPoses[80];
            float3 _Udon_MaxPoses[80];
            float _Udon_BlackedOutCount;

            float _VRChatMirrorMode;
            float3 _VRChatMirrorCameraPos;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld, v.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
                #ifdef USING_STEREO_MATRICES
                float3 cameraPos = (unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1]) * 0.5;
                #else
                float3 cameraPos = _WorldSpaceCameraPos;
                #endif

                o.cameraPos = _VRChatMirrorMode > 0 ? _VRChatMirrorCameraPos : cameraPos;

                o.depthTextureGrabPos = ComputeGrabScreenPos(o.vertex);
                o.rayFromCamera.xyz = o.world.xyz - o.cameraPos.xyz;
				o.rayFromCamera.w = dot(o.vertex, ObliqueFrustumCorrection); // oblique frustrum correction factor
                return o;
            }

            
			float insideBox3D(float3 v, float3 bottomLeft, float3 topRight) {
			    float3 s = step(bottomLeft, v) - step(topRight, v);
			    return s.x * s.y * s.z; 
			}

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );
            	
                float perspectiveDivide = 1.f / i.vertex.w;
				float4 rayFromCamera = i.rayFromCamera * perspectiveDivide;
				float2 depthTextureGrabPos = i.depthTextureGrabPos.xy * perspectiveDivide;

				float z = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, depthTextureGrabPos);

				#if UNITY_REVERSED_Z
				if (z == 0.f) {
				#else
				if (z == 1.f) {
				#endif
					discard;
					return fixed4(0,0,0,1);
				}

				// linearize depth and use it to calculate background world position
				float depth = CorrectedLinearEyeDepth(z, rayFromCamera.w);
				float3 worldPosition = rayFromCamera.xyz * depth + i.cameraPos.xyz;

                //return fixed4(worldPosition.xyz, 1);
                
                for (int l = 0; l < (int)_Udon_BlackedOutCount; ++l)
                {
                    float3 min = mul(_Udon_MinPoses[l], unity_ObjectToWorld);
                    float3 max = mul(_Udon_MaxPoses[l], unity_ObjectToWorld);
                    if (insideBox3D(worldPosition, min, max)) {
                        return fixed4(0,0,0,1);
                    }
                }
                
                discard;
                return fixed4(0,0,0,1);
            }
            ENDCG
        }
    }
}
