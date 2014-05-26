// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:Transparent/Bumped Diffuse,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31739,y:32375|custl-518-OUT,alpha-520-OUT;n:type:ShaderForge.SFN_Color,id:380,x:32562,y:32377,ptlb:Color,ptin:_Color,glob:False,c1:0.07586193,c2:0,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:505,x:32483,y:32570,ptlb:Alpha,ptin:_Alpha,min:0,cur:0.5639098,max:1;n:type:ShaderForge.SFN_NormalVector,id:506,x:32913,y:32609,pt:False;n:type:ShaderForge.SFN_Dot,id:508,x:32720,y:32742,dt:0|A-506-OUT,B-509-OUT;n:type:ShaderForge.SFN_ViewVector,id:509,x:32913,y:32773;n:type:ShaderForge.SFN_Step,id:510,x:32474,y:32795|A-522-OUT,B-508-OUT;n:type:ShaderForge.SFN_Multiply,id:518,x:32221,y:32466|A-380-RGB,B-510-OUT;n:type:ShaderForge.SFN_Multiply,id:519,x:32241,y:32628|A-505-OUT,B-510-OUT;n:type:ShaderForge.SFN_OneMinus,id:520,x:32059,y:32628|IN-519-OUT;n:type:ShaderForge.SFN_Slider,id:522,x:32499,y:32677,ptlb:BorderWidth,ptin:_BorderWidth,min:0,cur:0,max:1;proporder:380-505-522;pass:END;sub:END;*/

Shader "Shader Forge/hollow" {
    Properties {
        _Color ("Color", Color) = (0.07586193,0,1,1)
        _Alpha ("Alpha", Range(0, 1)) = 0.5639098
        _BorderWidth ("BorderWidth", Range(0, 1)) = 0
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
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Alpha;
            uniform float _BorderWidth;
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
                float node_510 = step(_BorderWidth,dot(i.normalDir,viewDirection));
                float3 finalColor = (_Color.rgb*node_510);
/// Final Color:
                return fixed4(finalColor,(1.0 - (_Alpha*node_510)));
            }
            ENDCG
        }
    }
    FallBack "Transparent/Bumped Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
