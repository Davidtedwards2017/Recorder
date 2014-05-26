// Shader created with Shader Forge Beta 0.33 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.33;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:30598,y:31655|diff-984-OUT,spec-984-OUT,emission-984-OUT,custl-984-OUT;n:type:ShaderForge.SFN_Time,id:77,x:34340,y:31534;n:type:ShaderForge.SFN_Sin,id:78,x:34089,y:31534|IN-77-TTR;n:type:ShaderForge.SFN_ConstantClamp,id:80,x:33616,y:31543,min:0,max:1|IN-2312-OUT;n:type:ShaderForge.SFN_Multiply,id:81,x:33434,y:31689|A-80-OUT,B-87-OUT;n:type:ShaderForge.SFN_Time,id:83,x:34253,y:31763;n:type:ShaderForge.SFN_Sin,id:85,x:34002,y:31763|IN-83-TDB;n:type:ShaderForge.SFN_ConstantClamp,id:87,x:33657,y:31766,min:0,max:1|IN-2374-OUT;n:type:ShaderForge.SFN_Multiply,id:88,x:33257,y:31554|A-80-OUT,B-81-OUT;n:type:ShaderForge.SFN_Panner,id:91,x:33543,y:30829,spu:0,spv:1.5;n:type:ShaderForge.SFN_Tex2d,id:92,x:33350,y:30829,ptlb:FlickerTexture2,ptin:_FlickerTexture2,tex:7ebcb2afe2fea6c4690e1b893c291fca,ntxv:0,isnm:False|UVIN-91-UVOUT;n:type:ShaderForge.SFN_Multiply,id:94,x:33267,y:31038|A-92-RGB,B-939-OUT;n:type:ShaderForge.SFN_Lerp,id:96,x:32916,y:31021|A-97-OUT,B-94-OUT,T-99-OUT;n:type:ShaderForge.SFN_Vector1,id:97,x:33147,y:30913,v1:0;n:type:ShaderForge.SFN_Desaturate,id:99,x:33144,y:31274|COL-1921-RGB;n:type:ShaderForge.SFN_Lerp,id:114,x:32555,y:31738|A-147-OUT,B-161-RGB,T-2431-OUT;n:type:ShaderForge.SFN_Add,id:115,x:32169,y:31210|A-96-OUT,B-114-OUT,C-1470-OUT;n:type:ShaderForge.SFN_Vector1,id:147,x:32868,y:31522,v1:0;n:type:ShaderForge.SFN_Tex2d,id:161,x:33027,y:31805,ptlb:node_161,ptin:_node_161,tex:26c22711225093d47bd4f1294ca52131,ntxv:0,isnm:False|UVIN-1530-UVOUT;n:type:ShaderForge.SFN_Color,id:858,x:31598,y:31840,ptlb:node_858,ptin:_node_858,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Fresnel,id:865,x:31702,y:32059|EXP-866-OUT;n:type:ShaderForge.SFN_Vector1,id:866,x:31986,y:32137,v1:3;n:type:ShaderForge.SFN_Multiply,id:875,x:31125,y:32081|A-858-RGB,B-865-OUT;n:type:ShaderForge.SFN_Divide,id:934,x:31359,y:32308|A-937-OUT,B-936-OUT;n:type:ShaderForge.SFN_Multiply,id:935,x:31740,y:32313|A-865-OUT,B-936-OUT;n:type:ShaderForge.SFN_Vector1,id:936,x:31797,y:32549,v1:10;n:type:ShaderForge.SFN_Ceil,id:937,x:31571,y:32249|IN-935-OUT;n:type:ShaderForge.SFN_Vector3,id:939,x:33438,y:31127,v1:1,v2:2,v3:4;n:type:ShaderForge.SFN_Add,id:984,x:31247,y:31669|A-985-OUT,B-875-OUT;n:type:ShaderForge.SFN_Multiply,id:985,x:31609,y:31519|A-115-OUT,B-858-RGB;n:type:ShaderForge.SFN_Panner,id:1464,x:33543,y:30430,spu:0,spv:1;n:type:ShaderForge.SFN_Tex2d,id:1466,x:33350,y:30430,ptlb:FlickerTexture1,ptin:_FlickerTexture1,ntxv:0,isnm:False|UVIN-1464-UVOUT;n:type:ShaderForge.SFN_Multiply,id:1468,x:33147,y:30576|A-1466-RGB,B-1476-OUT;n:type:ShaderForge.SFN_Lerp,id:1470,x:32916,y:30622|A-1472-OUT,B-1468-OUT,T-1474-OUT;n:type:ShaderForge.SFN_Vector1,id:1472,x:33147,y:30514,v1:0;n:type:ShaderForge.SFN_Desaturate,id:1474,x:33147,y:30735|COL-1921-RGB;n:type:ShaderForge.SFN_Vector3,id:1476,x:33350,y:30673,v1:1,v2:2,v3:4;n:type:ShaderForge.SFN_Panner,id:1530,x:33226,y:31909,spu:0.2,spv:0.1;n:type:ShaderForge.SFN_Tex2d,id:1921,x:33977,y:30936,ptlb:BaseTexture,ptin:_BaseTexture,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Divide,id:2312,x:33818,y:31431|A-78-OUT,B-2313-OUT;n:type:ShaderForge.SFN_Vector1,id:2313,x:33890,y:31643,v1:1;n:type:ShaderForge.SFN_Divide,id:2374,x:33828,y:31766|A-85-OUT,B-2382-OUT;n:type:ShaderForge.SFN_Vector1,id:2382,x:33907,y:31998,v1:3;n:type:ShaderForge.SFN_Add,id:2431,x:33073,y:31554|A-2433-OUT,B-88-OUT;n:type:ShaderForge.SFN_Vector1,id:2433,x:33334,y:31444,v1:0.2;proporder:92-161-858-1466-1921;pass:END;sub:END;*/

Shader "Shader Forge/Hologram" {
    Properties {
        _FlickerTexture2 ("FlickerTexture2", 2D) = "white" {}
        _node_161 ("node_161", 2D) = "white" {}
        _node_858 ("node_858", Color) = (1,0,0,1)
        _FlickerTexture1 ("FlickerTexture1", 2D) = "white" {}
        _BaseTexture ("BaseTexture", 2D) = "white" {}
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
            Cull Off
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
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _FlickerTexture2; uniform float4 _FlickerTexture2_ST;
            uniform sampler2D _node_161; uniform float4 _node_161_ST;
            uniform float4 _node_858;
            uniform sampler2D _FlickerTexture1; uniform float4 _FlickerTexture1_ST;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
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
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
////// Emissive:
                float node_97 = 0.0;
                float4 node_2481 = _Time + _TimeEditor;
                float2 node_2480 = i.uv0;
                float2 node_91 = (node_2480.rg+node_2481.g*float2(0,1.5));
                float4 node_1921 = tex2D(_BaseTexture,TRANSFORM_TEX(node_2480.rg, _BaseTexture));
                float node_147 = 0.0;
                float2 node_1530 = (node_2480.rg+node_2481.g*float2(0.2,0.1));
                float4 node_77 = _Time + _TimeEditor;
                float node_80 = clamp((sin(node_77.a)/1.0),0,1);
                float4 node_83 = _Time + _TimeEditor;
                float node_1472 = 0.0;
                float2 node_1464 = (node_2480.rg+node_2481.g*float2(0,1));
                float node_865 = pow(1.0-max(0,dot(normalDirection, viewDirection)),3.0);
                float3 node_984 = (((lerp(float3(node_97,node_97,node_97),(tex2D(_FlickerTexture2,TRANSFORM_TEX(node_91, _FlickerTexture2)).rgb*float3(1,2,4)),dot(node_1921.rgb,float3(0.3,0.59,0.11)))+lerp(float3(node_147,node_147,node_147),tex2D(_node_161,TRANSFORM_TEX(node_1530, _node_161)).rgb,(0.2+(node_80*(node_80*clamp((sin(node_83.b)/3.0),0,1)))))+lerp(float3(node_1472,node_1472,node_1472),(tex2D(_FlickerTexture1,TRANSFORM_TEX(node_1464, _FlickerTexture1)).rgb*float3(1,2,4)),dot(node_1921.rgb,float3(0.3,0.59,0.11))))*_node_858.rgb)+(_node_858.rgb*node_865));
                float3 emissive = node_984;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = node_984;
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * node_984;
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
