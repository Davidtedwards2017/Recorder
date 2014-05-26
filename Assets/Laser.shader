// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32504,y:32629|custl-9-RGB;n:type:ShaderForge.SFN_Color,id:9,x:33019,y:32892,ptlb:Color,ptin:_Color,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:122,x:35839,y:32770,uv:0;n:type:ShaderForge.SFN_Multiply,id:123,x:35499,y:32787|A-128-UVOUT,B-127-OUT;n:type:ShaderForge.SFN_Floor,id:124,x:35274,y:32845|IN-123-OUT;n:type:ShaderForge.SFN_Noise,id:125,x:34370,y:32527|XY-124-OUT;n:type:ShaderForge.SFN_Vector1,id:127,x:35723,y:32984,v1:5;n:type:ShaderForge.SFN_Panner,id:128,x:35661,y:32773,spu:0,spv:0|UVIN-122-UVOUT;n:type:ShaderForge.SFN_Time,id:130,x:34803,y:31823;n:type:ShaderForge.SFN_Sin,id:131,x:34544,y:31844|IN-130-TDB;n:type:ShaderForge.SFN_Multiply,id:134,x:32932,y:32563|A-644-OUT,B-9-RGB;n:type:ShaderForge.SFN_Abs,id:338,x:34216,y:31842|IN-131-OUT;n:type:ShaderForge.SFN_Time,id:356,x:34911,y:32162;n:type:ShaderForge.SFN_Abs,id:358,x:34358,y:32162|IN-394-OUT;n:type:ShaderForge.SFN_Sin,id:394,x:34545,y:32166|IN-356-TTR;n:type:ShaderForge.SFN_Noise,id:576,x:34363,y:32349|XY-604-OUT;n:type:ShaderForge.SFN_TexCoord,id:600,x:35904,y:32394,uv:0;n:type:ShaderForge.SFN_Multiply,id:602,x:35564,y:32411|A-608-UVOUT,B-606-OUT;n:type:ShaderForge.SFN_Floor,id:604,x:35339,y:32469|IN-602-OUT;n:type:ShaderForge.SFN_Vector1,id:606,x:35788,y:32608,v1:5;n:type:ShaderForge.SFN_Panner,id:608,x:35726,y:32397,spu:0,spv:0|UVIN-600-UVOUT;n:type:ShaderForge.SFN_Multiply,id:644,x:33575,y:32536|A-576-OUT,B-125-OUT;n:type:ShaderForge.SFN_TexCoord,id:653,x:33777,y:32881,uv:0;n:type:ShaderForge.SFN_TexCoord,id:654,x:33773,y:32869,uv:0;n:type:ShaderForge.SFN_Vector1,id:655,x:32818,y:33066,v1:0.1;proporder:9;pass:END;sub:END;*/

Shader "Shader Forge/Laser" {
    Properties {
        _Color ("Color", Color) = (1,0,0,1)
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
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float3 finalColor = _Color.rgb;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
