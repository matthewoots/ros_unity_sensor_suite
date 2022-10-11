// https://www.ronja-tutorials.com/post/017-postprocessing-depth/

Shader "Hidden/depth"
 {
     Properties
     {
        
     }
     SubShader
     {
         Pass
         {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			// uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;
 
			// The object data that's put into the vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // The data that's used to generate fragments and can be read by the fragment shader
            struct v2f{
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
             
            // The vertex shader
            v2f vert(appdata v){
                v2f o;
                // Convert the vertex positions from object space to clip space so they can be rendered
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // The fragment shader
            fixed4 frag(v2f i) : SV_TARGET
			{
                i.uv.y = 1 - i.uv.y;
                // Get depth from depth texture
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                // Linear depth between camera and far clipping plane
                depth = Linear01Depth(depth);
                // Depth as distance from camera in units
                // depth = depth * _ProjectionParams.z;
                
				return depth;
			}
             
             ENDCG
         }
     } 
 }
