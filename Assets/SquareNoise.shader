// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32564,y:33088|custl-27-OUT;n:type:ShaderForge.SFN_Color,id:6,x:33978,y:32822,ptlb:Color,ptin:_Color,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_NormalVector,id:24,x:34031,y:33557,pt:False;n:type:ShaderForge.SFN_Multiply,id:27,x:32948,y:33519|A-6-RGB,B-33-OUT;n:type:ShaderForge.SFN_Multiply,id:29,x:33659,y:33494|A-30-OUT,B-35-OUT;n:type:ShaderForge.SFN_Dot,id:30,x:33860,y:33372,dt:0|A-36-OUT,B-24-OUT;n:type:ShaderForge.SFN_Ceil,id:32,x:33428,y:33337|IN-29-OUT;n:type:ShaderForge.SFN_Divide,id:33,x:33209,y:33360|A-32-OUT,B-35-OUT;n:type:ShaderForge.SFN_Vector1,id:35,x:33560,y:33708,v1:10;n:type:ShaderForge.SFN_ViewVector,id:36,x:34023,y:33294;n:type:ShaderForge.SFN_TexCoord,id:42,x:34179,y:33068,uv:0;n:type:ShaderForge.SFN_TexCoord,id:45,x:34213,y:33031,uv:0;proporder:6;pass:END;sub:END;*/

Shader "Shader Forge/SquareNoise" {
    Properties {
        _Color ("Color", Color) = (1,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _Color;
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
                float node_35 = 10.0;
                float3 finalColor = (_Color.rgb*(ceil((dot(viewDirection,i.normalDir)*node_35))/node_35));
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
