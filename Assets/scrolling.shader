// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32355,y:32476|emission-194-OUT,custl-194-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33207,y:32726,ptlb:MainTex,ptin:_MainTex,tex:00c254830e3bf804e9961900cedf0958,ntxv:0,isnm:False|UVIN-5-OUT;n:type:ShaderForge.SFN_TexCoord,id:3,x:33884,y:32685,uv:0;n:type:ShaderForge.SFN_Panner,id:4,x:33695,y:32479,spu:0.02,spv:0.02|UVIN-3-UVOUT;n:type:ShaderForge.SFN_Multiply,id:5,x:33455,y:32685|A-4-UVOUT,B-6-OUT;n:type:ShaderForge.SFN_Vector1,id:6,x:33550,y:32978,v1:25;n:type:ShaderForge.SFN_Color,id:73,x:33218,y:32193,ptlb:Color,ptin:_Color,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:165,x:33097,y:32951,v1:10;n:type:ShaderForge.SFN_Power,id:170,x:33027,y:32701|VAL-2-RGB,EXP-165-OUT;n:type:ShaderForge.SFN_Multiply,id:194,x:32850,y:32397|A-73-RGB,B-170-OUT;proporder:2-73;pass:END;sub:END;*/

Shader "Shader Forge/scrolling" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
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
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
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
////// Emissive:
                float4 node_209 = _Time + _TimeEditor;
                float2 node_5 = ((i.uv0.rg+node_209.g*float2(0.02,0.02))*25.0);
                float3 node_194 = (_Color.rgb*pow(tex2D(_MainTex,TRANSFORM_TEX(node_5, _MainTex)).rgb,10.0));
                float3 emissive = node_194;
                float3 finalColor = emissive + node_194;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
