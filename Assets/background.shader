// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-2678-OUT,diffpow-3050-OUT,emission-2678-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:34000,y:33234,ptlb:Texture,ptin:_Texture,tex:00c254830e3bf804e9961900cedf0958,ntxv:0,isnm:False|UVIN-10-OUT;n:type:ShaderForge.SFN_TexCoord,id:9,x:34942,y:33021,uv:0;n:type:ShaderForge.SFN_Multiply,id:10,x:34367,y:33172|A-9-UVOUT,B-2709-OUT;n:type:ShaderForge.SFN_Multiply,id:14,x:34285,y:32327|A-2709-OUT,B-21-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:21,x:34599,y:32314,uv:0;n:type:ShaderForge.SFN_Tex2d,id:115,x:34086,y:32330,ptlb:Gra1diant Texture 2,ptin:_Gra1diantTexture2,tex:dbdba3360649d514bbd1677cc99405b9,ntxv:0,isnm:False|UVIN-14-OUT;n:type:ShaderForge.SFN_Multiply,id:116,x:33743,y:32254|A-160-OUT,B-115-RGB;n:type:ShaderForge.SFN_Tex2d,id:131,x:34502,y:31882,ptlb:Gradiant Texture,ptin:_GradiantTexture,tex:dbdba3360649d514bbd1677cc99405b9,ntxv:0,isnm:False|UVIN-705-UVOUT;n:type:ShaderForge.SFN_Power,id:160,x:33979,y:31972|VAL-131-RGB,EXP-163-OUT;n:type:ShaderForge.SFN_Vector1,id:163,x:34950,y:32370,v1:7;n:type:ShaderForge.SFN_Multiply,id:322,x:33401,y:33173|A-2-RGB,B-328-RGB;n:type:ShaderForge.SFN_Color,id:328,x:33605,y:33293,ptlb:Color,ptin:_Color,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Panner,id:705,x:34725,y:31882,spu:0,spv:0.15;n:type:ShaderForge.SFN_Color,id:812,x:33632,y:32497,ptlb:LightColor,ptin:_LightColor,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:988,x:33385,y:32274|A-812-RGB,B-116-OUT;n:type:ShaderForge.SFN_Add,id:2678,x:33160,y:32673|A-988-OUT,B-322-OUT;n:type:ShaderForge.SFN_Vector1,id:2709,x:34946,y:32732,v1:25;n:type:ShaderForge.SFN_Vector1,id:3050,x:33069,y:32920,v1:50;proporder:2-115-131-328-812;pass:END;sub:END;*/

Shader "Shader Forge/background" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Gra1diantTexture2 ("Gra1diant Texture 2", 2D) = "white" {}
        _GradiantTexture ("Gradiant Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _LightColor ("LightColor", Color) = (1,0,0,1)
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
            
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Gra1diantTexture2; uniform float4 _Gra1diantTexture2_ST;
            uniform sampler2D _GradiantTexture; uniform float4 _GradiantTexture_ST;
            uniform float4 _Color;
            uniform float4 _LightColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = pow(max( 0.0, NdotL), 50.0) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
////// Emissive:
                float4 node_3066 = _Time + _TimeEditor;
                float2 node_705 = (i.uv0.rg+node_3066.g*float2(0,0.15));
                float node_2709 = 25.0;
                float2 node_14 = (node_2709*i.uv0.rg);
                float2 node_10 = (i.uv0.rg*node_2709);
                float3 node_2678 = ((_LightColor.rgb*(pow(tex2D(_GradiantTexture,TRANSFORM_TEX(node_705, _GradiantTexture)).rgb,7.0)*tex2D(_Gra1diantTexture2,TRANSFORM_TEX(node_14, _Gra1diantTexture2)).rgb))+(tex2D(_Texture,TRANSFORM_TEX(node_10, _Texture)).rgb*_Color.rgb));
                float3 emissive = node_2678;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * node_2678;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Gra1diantTexture2; uniform float4 _Gra1diantTexture2_ST;
            uniform sampler2D _GradiantTexture; uniform float4 _GradiantTexture_ST;
            uniform float4 _Color;
            uniform float4 _LightColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = pow(max( 0.0, NdotL), 50.0) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_3068 = _Time + _TimeEditor;
                float2 node_705 = (i.uv0.rg+node_3068.g*float2(0,0.15));
                float node_2709 = 25.0;
                float2 node_14 = (node_2709*i.uv0.rg);
                float2 node_10 = (i.uv0.rg*node_2709);
                float3 node_2678 = ((_LightColor.rgb*(pow(tex2D(_GradiantTexture,TRANSFORM_TEX(node_705, _GradiantTexture)).rgb,7.0)*tex2D(_Gra1diantTexture2,TRANSFORM_TEX(node_14, _Gra1diantTexture2)).rgb))+(tex2D(_Texture,TRANSFORM_TEX(node_10, _Texture)).rgb*_Color.rgb));
                finalColor += diffuseLight * node_2678;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
