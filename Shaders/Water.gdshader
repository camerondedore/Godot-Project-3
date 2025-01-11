shader_type spatial;
render_mode depth_draw_always;

uniform vec3 waterAbsorbColor : source_color = vec3(1.0, 0.35, 0.0);
uniform vec3 waterFogColor : source_color;
uniform float roughness : hint_range(0.0, 1.0) = 0.05;
//uniform float edgeHardness : hint_range(1.0, 40.0) = 5;
uniform float uvScale = 30;
uniform float waveTimeScale : hint_range(0.0, 1.0) = 0.1;
uniform vec2 waveVelocity1 = vec2(0.1, 0.2);
uniform vec2 waveVelocity2 = vec2(-0.2, -0.1);
uniform vec2 waveVelocity3 = vec2(0.15, -0.05);
//uniform float refractionStrength : hint_range(0.0, 10.0) = 0.05;
//uniform float refractionDistance : hint_range(0.0, 10.0) = 0.05;
uniform float depthDistance = 4.0;
uniform float colorAbsorbtionDensity : hint_range(0.0, 10.0, 0.05) = 1.0;
uniform float fogStrength : hint_range(0.0, 1.0, 0.05) = 1.0;
uniform float beersLawFog = 1.0;
uniform float fogOffset = 0.0;
uniform float normalMapHeight = 0.5;
uniform sampler2D waterNormalMap1 : hint_normal;
uniform sampler2D waterNormalMap2 : hint_normal;
uniform sampler2D waterNormalMap3 : hint_normal;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture, filter_linear_mipmap;

//varying vec3 worldPosition;


void vertex()
{
	// get world position of vertex
	//worldPosition = NODE_POSITION_WORLD + VERTEX;
}

vec3 addNormals(vec3 a, vec3 b)
{
	a = a * 2.0 - 1.0;
	b = b * 2.0 - 1.0;
	vec3 c = a + b;
	c = c * 0.5 + 0.5;
	return c;
}

void fragment()
{
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec2 worldUvCoordinates = worldPosition.rb / uvScale;

	// get time offsets for waves
	vec2 waveOffset1 = TIME * waveVelocity1 * waveTimeScale;
	vec2 waveOffset2 = TIME * waveVelocity2 * waveTimeScale;
	vec2 waveOffset3 = TIME * waveVelocity3 * waveTimeScale;
	
	// get normal map from texture using world position for UV coordinated
	vec3 waterNormal1 = texture(waterNormalMap1, worldUvCoordinates + waveOffset1).rgb;
	vec3 waterNormal2 = texture(waterNormalMap2, worldUvCoordinates + waveOffset2).rgb;
	vec3 waterNormal3 = texture(waterNormalMap3, worldUvCoordinates + waveOffset3).rgb;
	vec3 combinedWaterNormal = addNormals(addNormals(waterNormal1, waterNormal2), waterNormal3);
	combinedWaterNormal = mix(vec3(0.5, 0.5, 1.0), combinedWaterNormal, normalMapHeight);
	
	// get refraction uvs
	//vec3 refractionNormal = combinedWaterNormal * 2.0 - 1.0;
	//vec3 refractionOffset = TANGENT * refractionNormal + BINORMAL * refractionNormal + NORMAL * refractionNormal;
	//vec2 refractedUv = SCREEN_UV - refractionOffset.xy * refractionStrength;
	
	// get depth and screen textures
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	//float refractedDepth = texture(DEPTH_TEXTURE, refractedUv).r;
	vec3 screenTexture = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;

	// get depth of water in world position Y depth
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	vec4 world = INV_VIEW_MATRIX * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	float depthTextureY = world.y / world.w;
	float vertexY = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).y;
	
	// get depth from view
	//vec3 ndcRefract = vec3(SCREEN_UV * 2.0 - 1.0, refractedDepth);
	//vec4 viewWorld = INV_PROJECTION_MATRIX * vec4(ndcRefract, 1.0);
	//viewWorld.z = viewWorld.z / viewWorld.w;
	//float linearDepth = -viewWorld.z + VERTEX.z;

	// get blend
	float depthBlend = clamp((vertexY - depthTextureY) / depthDistance, 0.0, 1.0);
	float depthBlendColorAbsorbtion = depthBlend * colorAbsorbtionDensity;
	float depthBlendFog = clamp((vertexY - depthTextureY + fogOffset) / depthDistance, 0.0, 1.0);
	depthBlendFog = exp(-(1.0 - depthBlendFog) * beersLawFog) * fogStrength;

	// get background color
	vec3 background = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0).rgb;
	// apply color absorbtion to background
	background = clamp(background - waterAbsorbColor * depthBlendColorAbsorbtion, 0.0, 1.0);

	// combine fog with background
	vec3 combinedBackgroundFog = mix(background, waterFogColor, depthBlendFog);

	// refraction old
	//vec3 unpackedNormal = combinedWaterNormal;
	//unpackedNormal.xy = unpackedNormal.xy * 2.0 - 1.0;
	//unpackedNormal.z = sqrt(max(0.0, 1.0 - dot(unpackedNormal.xy, unpackedNormal.xy)));
	//vec3 refractionNormal = normalize( mix(NORMAL,TANGENT * unpackedNormal.x + BINORMAL * unpackedNormal.y + NORMAL * unpackedNormal.z,NORMAL_MAP_DEPTH) );
	//vec2 depthRefractionUv = SCREEN_UV - refractionNormal.xy * refractionStrength;
	//
	//float refractDepthTexture = texture(DEPTH_TEXTURE, depthRefractionUv).r;
	//float refractDepth = PROJECTION_MATRIX[3][2] / (refractDepthTexture + PROJECTION_MATRIX[2][2]);
	//float refractDepthBlend = refractDepth + VERTEX.z;
	//
	//vec2 refractionUv = SCREEN_UV - refractionNormal.xy * refractionStrength * clamp(refractDepthBlend, 0.0, 1.0);
	//vec3 refractionTexture = texture(SCREEN_TEXTURE, refractionUv).rgb;
	//
	//
	//vec3 background = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;
	//vec3 combinedBackgroundFog = mix(refractionTexture, waterColor, depthForFog);


	// apply
	NORMAL_MAP = combinedWaterNormal;
	ROUGHNESS = roughness;
	ALBEDO = combinedBackgroundFog;
}