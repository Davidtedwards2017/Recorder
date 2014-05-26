// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|custl-12-OUT;n:type:ShaderForge.SFN_Fresnel,id:2,x:33969,y:32745;n:type:ShaderForge.SFN_Color,id:4,x:34007,y:32525,ptlb:node_4,ptin:_node_4,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5,x:33734,y:32690|A-4-RGB,B-2-OUT;n:type:ShaderForge.SFN_Multiply,id:6,x:33455,y:32856|A-5-OUT,B-8-OUT;n:type:ShaderForge.SFN_Slider,id:8,x:33728,y:33017,ptlb:node_8,ptin:_node_8,min:0,cur:1.32594,max:20;n:type:ShaderForge.SFN_Sin,id:10,x:33806,y:33288|IN-11-TTR;n:type:ShaderForge.SFN_Time,id:11,x:34036,y:33313;n:type:ShaderForge.SFN_Multiply,id:12,x:33191,y:33033|A-6-OUT,B-15-OUT;n:type:ShaderForge.SFN_Abs,id:13,x:33504,y:33305|IN-10-OUT;n:type:ShaderForge.SFN_Add,id:15,x:33251,y:33409|A-13-OUT,B-16-OUT;n:type:ShaderForge.SFN_Vector1,id:16,x:33425,y:33523,v1:0.1;proporder:4-8;pass:END;sub:END;*/

Shader "Shader Forge/ShieldShander" {
    Properties {
        _node_4 ("node_4", Color) = (1,0,0,1)
        _node_8 ("node_8", Range(0, 20)) = 1.32594
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
            Blend One One
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
            uniform float4 _node_4;
            uniform float _node_8;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
////// Lighting:
                float3 node_6 = ((_node_4.rgb*(1.0-max(0,dot(normalDirection, viewDirection))))*_node_8);
                float4 node_11 = _Time + _TimeEditor;
                float3 finalColor = (node_6*(abs(sin(node_11.a))+0.1));
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
