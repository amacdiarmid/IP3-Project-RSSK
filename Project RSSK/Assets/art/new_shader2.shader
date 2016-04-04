// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4770,x:32719,y:32712,varname:node_4770,prsc:2|diff-2381-OUT,spec-4304-OUT,gloss-142-OUT,normal-9908-RGB,emission-9442-OUT;n:type:ShaderForge.SFN_Tex2d,id:863,x:32146,y:32257,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_863,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c3edd55707701f14fbd19c74377a0e91,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9908,x:31939,y:33089,ptovrint:False,ptlb:normal,ptin:_normal,varname:node_9908,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7f2a155ca63365b41bf4dcef5b3a0a1d,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:6777,x:31682,y:32575,ptovrint:False,ptlb:spec,ptin:_spec,varname:node_6777,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:43ffc95ee1262e14ab6e678c7056400e,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4304,x:32076,y:32769,varname:node_4304,prsc:2|A-6777-RGB,B-6189-OUT,C-7389-RGB;n:type:ShaderForge.SFN_Slider,id:6189,x:31622,y:32794,ptovrint:False,ptlb:spec_intens,ptin:_spec_intens,varname:node_6189,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3.247863,max:10;n:type:ShaderForge.SFN_Color,id:8210,x:32197,y:32462,ptovrint:False,ptlb:diffuse_color,ptin:_diffuse_color,varname:node_8210,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8970588,c2:0.8970588,c3:0.8970588,c4:1;n:type:ShaderForge.SFN_Multiply,id:2381,x:32391,y:32312,varname:node_2381,prsc:2|A-863-RGB,B-8210-RGB;n:type:ShaderForge.SFN_Color,id:7389,x:31733,y:32923,ptovrint:False,ptlb:spec_color,ptin:_spec_color,varname:node_7389,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6029412,c2:0.5946248,c3:0.5275735,c4:1;n:type:ShaderForge.SFN_Slider,id:142,x:32195,y:33114,ptovrint:False,ptlb:gloss_slider,ptin:_gloss_slider,varname:node_142,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8606071,max:1;n:type:ShaderForge.SFN_Cubemap,id:6593,x:33051,y:32783,ptovrint:False,ptlb:cubemap,ptin:_cubemap,varname:node_6593,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,cube:a65475ebd49292b45b16e6f7c17788df,pvfc:0;n:type:ShaderForge.SFN_Multiply,id:8040,x:33127,y:32571,varname:node_8040,prsc:2|A-3358-OUT,B-8916-OUT;n:type:ShaderForge.SFN_Multiply,id:8916,x:33351,y:32820,varname:node_8916,prsc:2|A-6593-RGB,B-6593-A,C-5028-OUT;n:type:ShaderForge.SFN_Multiply,id:7297,x:33236,y:33183,varname:node_7297,prsc:2|A-2301-OUT,B-7734-OUT;n:type:ShaderForge.SFN_Vector1,id:3358,x:33330,y:32427,varname:node_3358,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Vector1,id:5028,x:33068,y:32969,varname:node_5028,prsc:2,v1:3;n:type:ShaderForge.SFN_Lerp,id:6005,x:33587,y:32638,varname:node_6005,prsc:2|A-8040-OUT,B-8916-OUT,T-5907-OUT;n:type:ShaderForge.SFN_NormalVector,id:2301,x:32970,y:33174,prsc:2,pt:True;n:type:ShaderForge.SFN_Vector1,id:7734,x:33017,y:33370,varname:node_7734,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Add,id:4487,x:33542,y:33261,varname:node_4487,prsc:2|A-7297-OUT,B-5090-OUT;n:type:ShaderForge.SFN_OneMinus,id:5090,x:33236,y:33428,varname:node_5090,prsc:2|IN-7734-OUT;n:type:ShaderForge.SFN_Multiply,id:6934,x:33808,y:32967,varname:node_6934,prsc:2|A-6005-OUT,B-4487-OUT,C-2606-OUT;n:type:ShaderForge.SFN_Slider,id:2606,x:33463,y:33452,ptovrint:False,ptlb:cubemap_control,ptin:_cubemap_control,varname:node_2606,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8333355,max:1;n:type:ShaderForge.SFN_Fresnel,id:5907,x:31555,y:33346,varname:node_5907,prsc:2;n:type:ShaderForge.SFN_Multiply,id:750,x:31897,y:33371,varname:node_750,prsc:2|A-9908-RGB,B-5907-OUT;n:type:ShaderForge.SFN_Multiply,id:4852,x:32006,y:33552,varname:node_4852,prsc:2|A-750-OUT,B-8702-OUT;n:type:ShaderForge.SFN_Slider,id:8702,x:31575,y:33534,ptovrint:False,ptlb:sss_str,ptin:_sss_str,varname:node_8702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Power,id:3387,x:32240,y:33604,varname:node_3387,prsc:2|VAL-4852-OUT,EXP-9076-OUT;n:type:ShaderForge.SFN_Slider,id:9076,x:31783,y:33717,ptovrint:False,ptlb:sss_coverage,ptin:_sss_coverage,varname:node_9076,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3655484,max:1;n:type:ShaderForge.SFN_Multiply,id:8041,x:32491,y:33514,varname:node_8041,prsc:2|A-3387-OUT,B-6940-RGB;n:type:ShaderForge.SFN_Color,id:6940,x:32170,y:33794,ptovrint:False,ptlb:sss_color,ptin:_sss_color,varname:node_6940,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8014706,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7358,x:32630,y:33706,varname:node_7358,prsc:2|A-8041-OUT,B-5548-G;n:type:ShaderForge.SFN_Tex2d,id:5548,x:32343,y:33902,ptovrint:False,ptlb:RGB MASK,ptin:_RGBMASK,varname:node_5548,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d471bf9ace168624faf38e0acace6080,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:9442,x:32718,y:33200,varname:node_9442,prsc:2|A-9065-OUT,B-7358-OUT,C-8554-OUT,D-7815-OUT;n:type:ShaderForge.SFN_Multiply,id:9065,x:32887,y:33599,varname:node_9065,prsc:2|A-7804-RGB,B-863-RGB;n:type:ShaderForge.SFN_AmbientLight,id:7804,x:32673,y:33447,varname:node_7804,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1876,x:34054,y:32743,varname:node_1876,prsc:2|A-863-R,B-6934-OUT,C-1503-OUT;n:type:ShaderForge.SFN_Slider,id:1503,x:33808,y:33149,ptovrint:False,ptlb:B_CM_Str,ptin:_B_CM_Str,varname:node_1503,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:1357,x:34044,y:32466,varname:node_1357,prsc:2|A-863-R,B-6934-OUT,C-1278-OUT;n:type:ShaderForge.SFN_Slider,id:1278,x:33815,y:32315,ptovrint:False,ptlb:R_CM_Str,ptin:_R_CM_Str,varname:node_1278,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:8554,x:34253,y:32510,varname:node_8554,prsc:2|A-5548-R,B-1357-OUT;n:type:ShaderForge.SFN_Multiply,id:7815,x:34270,y:32698,varname:node_7815,prsc:2|A-5548-B,B-1876-OUT;proporder:863-9908-6777-6189-7389-8210-142-6593-2606-8702-9076-6940-5548-1278-1503;pass:END;sub:END;*/

Shader "Custom/new_shader2" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _normal ("normal", 2D) = "bump" {}
        _spec ("spec", 2D) = "black" {}
        _spec_intens ("spec_intens", Range(0, 10)) = 3.247863
        _spec_color ("spec_color", Color) = (0.6029412,0.5946248,0.5275735,1)
        _diffuse_color ("diffuse_color", Color) = (0.8970588,0.8970588,0.8970588,1)
        _gloss_slider ("gloss_slider", Range(0, 1)) = 0.8606071
        _cubemap ("cubemap", Cube) = "_Skybox" {}
        _cubemap_control ("cubemap_control", Range(0, 1)) = 0.8333355
        _sss_str ("sss_str", Range(0, 1)) = 1
        _sss_coverage ("sss_coverage", Range(0, 1)) = 0.3655484
        _sss_color ("sss_color", Color) = (0.8014706,0,0,1)
        _RGBMASK ("RGB MASK", 2D) = "white" {}
        _R_CM_Str ("R_CM_Str", Range(0, 1)) = 0
        _B_CM_Str ("B_CM_Str", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
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
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform sampler2D _spec; uniform float4 _spec_ST;
            uniform float _spec_intens;
            uniform float4 _diffuse_color;
            uniform float4 _spec_color;
            uniform float _gloss_slider;
            uniform samplerCUBE _cubemap;
            uniform float _cubemap_control;
            uniform float _sss_str;
            uniform float _sss_coverage;
            uniform float4 _sss_color;
            uniform sampler2D _RGBMASK; uniform float4 _RGBMASK_ST;
            uniform float _B_CM_Str;
            uniform float _R_CM_Str;
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
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _normal_var = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(i.uv0, _normal)));
                float3 normalLocal = _normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss_slider;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _spec_var = tex2D(_spec,TRANSFORM_TEX(i.uv0, _spec));
                float3 specularColor = (_spec_var.rgb*_spec_intens*_spec_color.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(i.uv0, _diffuse));
                float3 diffuseColor = (_diffuse_var.rgb*_diffuse_color.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float node_5907 = (1.0-max(0,dot(normalDirection, viewDirection)));
                float4 _RGBMASK_var = tex2D(_RGBMASK,TRANSFORM_TEX(i.uv0, _RGBMASK));
                float4 _cubemap_var = texCUBE(_cubemap,viewReflectDirection);
                float3 node_8916 = (_cubemap_var.rgb*_cubemap_var.a*3.0);
                float node_7734 = 0.1;
                float3 node_6934 = (lerp((0.7*node_8916),node_8916,node_5907)*((normalDirection*node_7734)+(1.0 - node_7734))*_cubemap_control);
                float3 emissive = ((UNITY_LIGHTMODEL_AMBIENT.rgb*_diffuse_var.rgb)+((pow(((_normal_var.rgb*node_5907)*_sss_str),_sss_coverage)*_sss_color.rgb)*_RGBMASK_var.g)+(_RGBMASK_var.r*(_diffuse_var.r*node_6934*_R_CM_Str))+(_RGBMASK_var.b*(_diffuse_var.r*node_6934*_B_CM_Str)));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform sampler2D _spec; uniform float4 _spec_ST;
            uniform float _spec_intens;
            uniform float4 _diffuse_color;
            uniform float4 _spec_color;
            uniform float _gloss_slider;
            uniform samplerCUBE _cubemap;
            uniform float _cubemap_control;
            uniform float _sss_str;
            uniform float _sss_coverage;
            uniform float4 _sss_color;
            uniform sampler2D _RGBMASK; uniform float4 _RGBMASK_ST;
            uniform float _B_CM_Str;
            uniform float _R_CM_Str;
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
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _normal_var = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(i.uv0, _normal)));
                float3 normalLocal = _normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss_slider;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _spec_var = tex2D(_spec,TRANSFORM_TEX(i.uv0, _spec));
                float3 specularColor = (_spec_var.rgb*_spec_intens*_spec_color.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(i.uv0, _diffuse));
                float3 diffuseColor = (_diffuse_var.rgb*_diffuse_color.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
