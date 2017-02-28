// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:Bumped Specular,lico:1,lgpr:1,nrmq:1,limd:1,uamb:False,mssp:True,lmpd:True,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32579,y:32793|diff-2-OUT,spec-18-OUT,gloss-25-OUT,normal-103-OUT,emission-70-OUT;n:type:ShaderForge.SFN_Multiply,id:2,x:33013,y:32399|A-12-RGB,B-5-OUT,C-11-RGB;n:type:ShaderForge.SFN_Multiply,id:3,x:32272,y:32725|A-4-OUT,B-7-OUT;n:type:ShaderForge.SFN_Lerp,id:4,x:32272,y:32859|A-10-OUT,B-8-A,T-9-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:5,x:32332,y:32541,ptlb:AO Active,ptin:_AOActive,on:False|A-6-OUT,B-230-OUT;n:type:ShaderForge.SFN_Vector1,id:6,x:32922,y:32687,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:7,x:32439,y:32759,ptlb:AO Multiplier,ptin:_AOMultiplier,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:8,x:32272,y:33007,ptlb:AO Map (Alpha),ptin:_AOMapAlpha,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:9,x:32439,y:32962,ptlb:AO Burn,ptin:_AOBurn,glob:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:10,x:32922,y:32627,v1:0;n:type:ShaderForge.SFN_Color,id:11,x:33243,y:32296,ptlb:Main Color,ptin:_MainColor,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:12,x:33243,y:32456,ptlb:Base,ptin:_Base,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:13,x:33320,y:32668,ptlb:Specular Custom Map,ptin:_SpecularCustomMap,ntxv:0,isnm:False;n:type:ShaderForge.SFN_SwitchProperty,id:14,x:33168,y:32668,ptlb:Custom Specular,ptin:_CustomSpecular,on:False|A-12-A,B-13-A;n:type:ShaderForge.SFN_Lerp,id:15,x:32993,y:32632|A-10-OUT,B-14-OUT,T-17-OUT;n:type:ShaderForge.SFN_ValueProperty,id:17,x:33168,y:32830,ptlb:Spec Power,ptin:_SpecPower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:18,x:32940,y:32784|A-15-OUT,B-5-OUT,C-19-OUT,D-124-RGB;n:type:ShaderForge.SFN_ValueProperty,id:19,x:33168,y:32903,ptlb:Spec Burn,ptin:_SpecBurn,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:20,x:33478,y:32871,ptlb:Gloss,ptin:_Gloss,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:21,x:33302,y:32988|A-20-A,B-24-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:33399,y:33067,ptlb:Shininess,ptin:_Shininess,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:25,x:33145,y:32998,ptlb:Use Gloss Map,ptin:_UseGlossMap,on:False|A-24-OUT,B-21-OUT;n:type:ShaderForge.SFN_Tex2d,id:64,x:33505,y:33406,ptlb:Reflection Mask,ptin:_ReflectionMask,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Cubemap,id:65,x:33696,y:33394,ptlb:Reflection,ptin:_Reflection,cube:a596436b21c6d484bb9b3b6385e3e666,pvfc:0;n:type:ShaderForge.SFN_Multiply,id:70,x:33303,y:33438|A-65-RGB,B-71-OUT,C-64-A;n:type:ShaderForge.SFN_ValueProperty,id:71,x:33347,y:33366,ptlb:Reflection Power,ptin:_ReflectionPower,glob:False,v1:1;n:type:ShaderForge.SFN_Lerp,id:103,x:33703,y:32460|A-104-RGB,B-105-OUT,T-106-OUT;n:type:ShaderForge.SFN_Tex2d,id:104,x:33934,y:32282,ptlb:Normal,ptin:_Normal,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Vector3,id:105,x:33934,y:32460,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Slider,id:106,x:33934,y:32580,ptlb:Normal Burn,ptin:_NormalBurn,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:124,x:33710,y:32715,ptlb:Specular Color,ptin:_SpecularColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Max,id:229,x:31901,y:32800|A-3-OUT,B-10-OUT;n:type:ShaderForge.SFN_Min,id:230,x:31901,y:32664|A-229-OUT,B-6-OUT;proporder:11-12-104-106-124-17-19-14-13-24-25-20-5-8-7-9-71-65-64;pass:END;sub:END;*/

Shader "ManyWorlds/Mobile/BumpedSpecularRef" {
    Properties {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _Base ("Base", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _NormalBurn ("Normal Burn", Range(-1, 1)) = 0
        _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
        _SpecPower ("Spec Power", Float ) = 1
        _SpecBurn ("Spec Burn", Float ) = 1
        [MaterialToggle] _CustomSpecular ("Custom Specular", Float ) = 1
        _SpecularCustomMap ("Specular Custom Map", 2D) = "white" {}
        _Shininess ("Shininess", Range(0, 1)) = 0.5
        [MaterialToggle] _UseGlossMap ("Use Gloss Map", Float ) = 0.5
        _Gloss ("Gloss", 2D) = "white" {}
        [MaterialToggle] _AOActive ("AO Active", Float ) = 1
        _AOMapAlpha ("AO Map (Alpha)", 2D) = "white" {}
        _AOMultiplier ("AO Multiplier", Float ) = 1
        _AOBurn ("AO Burn", Float ) = 1
        _ReflectionPower ("Reflection Power", Float ) = 1
        _Reflection ("Reflection", Cube) = "_Skybox" {}
        _ReflectionMask ("Reflection Mask", 2D) = "white" {}
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
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform fixed _AOActive;
            uniform float _AOMultiplier;
            uniform sampler2D _AOMapAlpha; uniform float4 _AOMapAlpha_ST;
            uniform float _AOBurn;
            uniform float4 _MainColor;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _SpecularCustomMap; uniform float4 _SpecularCustomMap_ST;
            uniform fixed _CustomSpecular;
            uniform float _SpecPower;
            uniform float _SpecBurn;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform float _Shininess;
            uniform fixed _UseGlossMap;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform samplerCUBE _Reflection;
            uniform float _ReflectionPower;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _NormalBurn;
            uniform float4 _SpecularColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_243 = i.uv0;
                float3 normalLocal = lerp(UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_243.rg, _Normal))).rgb,float3(0,0,1),_NormalBurn);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, normalLocal));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(lmtex);
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                #ifndef LIGHTMAP_OFF
                    float3 diffuse = lightmap.rgb;
                #else
                    float3 diffuse = max( 0.0, NdotL) * attenColor;
                #endif
////// Emissive:
                float3 emissive = (texCUBE(_Reflection,viewReflectDirection).rgb*_ReflectionPower*tex2D(_ReflectionMask,TRANSFORM_TEX(node_243.rg, _ReflectionMask)).a);
///////// Gloss:
                float gloss = lerp( _Shininess, (tex2D(_Gloss,TRANSFORM_TEX(node_243.rg, _Gloss)).a*_Shininess), _UseGlossMap );
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float node_10 = 0.0;
                float4 node_12 = tex2D(_Base,TRANSFORM_TEX(node_243.rg, _Base));
                float node_6 = 1.0;
                float node_5 = lerp( node_6, min(max((lerp(node_10,tex2D(_AOMapAlpha,TRANSFORM_TEX(node_243.rg, _AOMapAlpha)).a,_AOBurn)*_AOMultiplier),node_10),node_6), _AOActive );
                float3 specularColor = (lerp(node_10,lerp( node_12.a, tex2D(_SpecularCustomMap,TRANSFORM_TEX(node_243.rg, _SpecularCustomMap)).a, _CustomSpecular ),_SpecPower)*node_5*_SpecBurn*_SpecularColor.rgb);
                float3 specular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                #ifndef LIGHTMAP_OFF
                    #ifndef DIRLIGHTMAP_OFF
                        specular *= lightmap;
                    #else
                        specular *= (floor(attenuation) * _LightColor0.xyz);
                    #endif
                #else
                    specular *= (floor(attenuation) * _LightColor0.xyz);
                #endif
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * (node_12.rgb*node_5*_MainColor.rgb);
                finalColor += specular;
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
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform fixed _AOActive;
            uniform float _AOMultiplier;
            uniform sampler2D _AOMapAlpha; uniform float4 _AOMapAlpha_ST;
            uniform float _AOBurn;
            uniform float4 _MainColor;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _SpecularCustomMap; uniform float4 _SpecularCustomMap_ST;
            uniform fixed _CustomSpecular;
            uniform float _SpecPower;
            uniform float _SpecBurn;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform float _Shininess;
            uniform fixed _UseGlossMap;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform samplerCUBE _Reflection;
            uniform float _ReflectionPower;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _NormalBurn;
            uniform float4 _SpecularColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_244 = i.uv0;
                float3 normalLocal = lerp(UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_244.rg, _Normal))).rgb,float3(0,0,1),_NormalBurn);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
///////// Gloss:
                float gloss = lerp( _Shininess, (tex2D(_Gloss,TRANSFORM_TEX(node_244.rg, _Gloss)).a*_Shininess), _UseGlossMap );
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float node_10 = 0.0;
                float4 node_12 = tex2D(_Base,TRANSFORM_TEX(node_244.rg, _Base));
                float node_6 = 1.0;
                float node_5 = lerp( node_6, min(max((lerp(node_10,tex2D(_AOMapAlpha,TRANSFORM_TEX(node_244.rg, _AOMapAlpha)).a,_AOBurn)*_AOMultiplier),node_10),node_6), _AOActive );
                float3 specularColor = (lerp(node_10,lerp( node_12.a, tex2D(_SpecularCustomMap,TRANSFORM_TEX(node_244.rg, _SpecularCustomMap)).a, _CustomSpecular ),_SpecPower)*node_5*_SpecBurn*_SpecularColor.rgb);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * (node_12.rgb*node_5*_MainColor.rgb);
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Bumped Specular"
    CustomEditor "ShaderForgeMaterialInspector"
}
