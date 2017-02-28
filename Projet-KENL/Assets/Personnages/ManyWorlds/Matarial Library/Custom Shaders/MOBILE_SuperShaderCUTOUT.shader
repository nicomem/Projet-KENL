// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:Bumped Specular,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32787,y:32871|diff-2-RGB,spec-62-OUT,gloss-71-OUT,normal-56-RGB,emission-50-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33231,y:32457,ptlb:Base,ptin:_Base,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:24,x:33527,y:32661,ptlb:Gloss(R)Rim(G)Ref(B),ptin:_GlossRRimGRefB,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Fresnel,id:30,x:33391,y:33022|EXP-34-OUT;n:type:ShaderForge.SFN_Multiply,id:31,x:33232,y:32970|A-24-G,B-30-OUT,C-32-OUT,D-91-RGB;n:type:ShaderForge.SFN_ValueProperty,id:32,x:33401,y:33172,ptlb:Rim Power,ptin:_RimPower,glob:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:34,x:33583,y:33086,v1:2;n:type:ShaderForge.SFN_Cubemap,id:46,x:33320,y:33310,ptlb:Reflection,ptin:_Reflection;n:type:ShaderForge.SFN_Multiply,id:47,x:33193,y:33138|A-24-B,B-46-RGB,C-49-OUT;n:type:ShaderForge.SFN_ValueProperty,id:49,x:33452,y:33283,ptlb:Ref Level,ptin:_RefLevel,glob:False,v1:1;n:type:ShaderForge.SFN_Add,id:50,x:33049,y:32970|A-31-OUT,B-47-OUT;n:type:ShaderForge.SFN_Tex2d,id:56,x:33115,y:33329,ptlb:Normal Map,ptin:_NormalMap,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Multiply,id:62,x:33074,y:32679|A-100-A,B-64-OUT;n:type:ShaderForge.SFN_ValueProperty,id:64,x:33231,y:32723,ptlb:Specular Level,ptin:_SpecularLevel,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:71,x:33218,y:32824|A-24-R,B-72-OUT;n:type:ShaderForge.SFN_ValueProperty,id:72,x:33541,y:32904,ptlb:Gloss Level,ptin:_GlossLevel,glob:False,v1:0.5;n:type:ShaderForge.SFN_Color,id:91,x:33583,y:33172,ptlb:Rim Color,ptin:_RimColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:100,x:33389,y:32509,ptlb:Specular Map,ptin:_SpecularMap,ntxv:0,isnm:False;proporder:2-56-100-64-72-24-32-91-46-49;pass:END;sub:END;*/

Shader "ManyWorlds/Mobile/SuperShader/SuperShader2SIDED" {
    Properties {
        _Base ("Base", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _SpecularMap ("Specular Map", 2D) = "white" {}
        _SpecularLevel ("Specular Level", Float ) = 1
        _GlossLevel ("Gloss Level", Float ) = 0.5
        _GlossRRimGRefB ("Gloss(R)Rim(G)Ref(B)", 2D) = "white" {}
        _RimPower ("Rim Power", Float ) = 1
        _RimColor ("Rim Color", Color) = (0.5,0.5,0.5,1)
        _Reflection ("Reflection", Cube) = "_Skybox" {}
        _RefLevel ("Ref Level", Float ) = 1
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
            Cull Off
            
            
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
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _GlossRRimGRefB; uniform float4 _GlossRRimGRefB_ST;
            uniform float _RimPower;
            uniform samplerCUBE _Reflection;
            uniform float _RefLevel;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SpecularLevel;
            uniform float _GlossLevel;
            uniform float4 _RimColor;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
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
                float2 node_154 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_154.rg, _NormalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
////// Emissive:
                float4 node_24 = tex2D(_GlossRRimGRefB,TRANSFORM_TEX(node_154.rg, _GlossRRimGRefB));
                float3 emissive = ((node_24.g*pow(1.0-max(0,dot(normalDirection, viewDirection)),2.0)*_RimPower*_RimColor.rgb)+(node_24.b*texCUBE(_Reflection,viewReflectDirection).rgb*_RefLevel));
///////// Gloss:
                float gloss = (node_24.r*_GlossLevel);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float node_62 = (tex2D(_SpecularMap,TRANSFORM_TEX(node_154.rg, _SpecularMap)).a*_SpecularLevel);
                float3 specularColor = float3(node_62,node_62,node_62);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * tex2D(_Base,TRANSFORM_TEX(node_154.rg, _Base)).rgb;
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
            Cull Off
            
            
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
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _GlossRRimGRefB; uniform float4 _GlossRRimGRefB_ST;
            uniform float _RimPower;
            uniform samplerCUBE _Reflection;
            uniform float _RefLevel;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SpecularLevel;
            uniform float _GlossLevel;
            uniform float4 _RimColor;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
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
                float2 node_155 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_155.rg, _NormalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
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
                float4 node_24 = tex2D(_GlossRRimGRefB,TRANSFORM_TEX(node_155.rg, _GlossRRimGRefB));
                float gloss = (node_24.r*_GlossLevel);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float node_62 = (tex2D(_SpecularMap,TRANSFORM_TEX(node_155.rg, _SpecularMap)).a*_SpecularLevel);
                float3 specularColor = float3(node_62,node_62,node_62);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * tex2D(_Base,TRANSFORM_TEX(node_155.rg, _Base)).rgb;
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
