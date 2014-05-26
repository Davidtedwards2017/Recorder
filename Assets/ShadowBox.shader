// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|custl-93-OUT,alpha-4-R;n:type:ShaderForge.SFN_Tex2d,id:4,x:33450,y:32599,ptlb:Texture,ptin:_Texture,tex:4f3f351e618fae64a9ca2847f90d2691,ntxv:0,isnm:False;n:type:ShaderForge.SFN_OneMinus,id:70,x:33131,y:32666|IN-4-RGB;n:type:ShaderForge.SFN_Multiply,id:93,x:33106,y:32476|A-94-RGB,B-4-RGB;n:type:ShaderForge.SFN_Color,id:94,x:33352,y:32334,ptlb:node_94,ptin:_node_94,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:4-94;pass:END;sub:END;*/

Shader "Shader Forge/ShadowBox" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _node_94 ("node_94", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _node_94;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float2 node_98 = i.uv0;
                float4 node_4 = tex2D(_Texture,TRANSFORM_TEX(node_98.rg, _Texture));
                float3 finalColor = (_node_94.rgb*node_4.rgb);
/// Final Color:
                return fixed4(finalColor,node_4.r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
