shader_type spatial;

uniform vec3 waterColor : source_color;
uniform vec3 foamColor : source_color;
uniform float foamSharpness = 1;
uniform float roughness : hint_range(0.0, 1.0) = 0.05;
uniform float uvScale = 5;
uniform float waveTimeScale : hint_range(0.0, 1.0) = 0.25;
uniform vec2 waveVelocity1 = vec2(0.1, 0.2);
uniform vec2 waveVelocity2 = vec2(-0.2, -0.1);
uniform vec2 waveVelocity3 = vec2(0.15, -0.05);
//uniform float refractionStrength : hint_range(0.0, 0.5) = 0.05;
uniform float depthOffset = -0.75;
uniform float beersLaw = 2.0;
uniform float heightAmount = 5;
uniform float heightStrength = 2;
uniform sampler2D waterNormalMap1;
uniform sampler2D waterNormalMap2;
uniform sampler2D waterNormalMap3;
uniform sampler2D waterHeightMap1;
uniform sampler2D waterHeightMap2;
uniform sampler2D waterHeightMap3;
uniform sampler2D waterFoamTexture1;
uniform sampler2D waterFoamTexture2;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture, filter_linear_mipmap;


void vertex() 
{
	
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
	
	// get depth of water
	float depthTexture = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	float depth = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[2][2]);
	float depthBlend = depth + VERTEX.z;
	
	// fog
	float depthForFog = exp((1.0 - depthBlend + depthOffset) * -beersLaw);
	depthForFog = clamp(depthForFog, 0.0, 1.0);	
	
	// get normal map from texture using world position for UV coordinated
	vec3 waterNormal1 = texture(waterNormalMap1, worldUvCoordinates + waveOffset1).rgb;
	vec3 waterNormal2 = texture(waterNormalMap2, worldUvCoordinates + waveOffset2).rgb;
	vec3 waterNormal3 = texture(waterNormalMap3, worldUvCoordinates + waveOffset3).rgb;
	//vec3 combinedWaterNormal = mix(waterNormal3, mix(waterNormal1, waterNormal2, 0.5), 0.5);
	vec3 combinedWaterNormal = normalize(waterNormal3 + waterNormal1 + waterNormal2);
	
	// get height from texture using world position for UV coordinated
	float waterHeight1 = texture(waterHeightMap1, worldUvCoordinates + waveOffset1).r;
	float waterHeight2 = texture(waterHeightMap2, worldUvCoordinates + waveOffset2).r;
	float waterHeight3 = texture(waterHeightMap3, worldUvCoordinates + waveOffset3).r;
	float combinedWaterHeight = (waterHeight1 + waterHeight2 + waterHeight3) * 0.33;
	combinedWaterHeight = pow(combinedWaterHeight, heightAmount);
	vec3 waterHeight = waterColor * combinedWaterHeight * heightStrength;
	
	
	// foam
	float foamBlend = clamp(smoothstep(0.25, 1.0, 1.0 - depthBlend * foamSharpness), 0.0, 1.0);
	float foam1 = texture(waterFoamTexture1, worldUvCoordinates + waveOffset1).r;
	float foam2 = texture(waterFoamTexture2, -worldUvCoordinates + waveOffset2).r;
	float foamCombined = clamp(smoothstep(0.1, 1.0, (foam1 + foam2) * 0.5), 0.0, 1.0);
	vec3 foamTexture = foamColor * foamCombined * foamBlend;
	
	// refraction
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
	
	vec3 background = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;
	vec3 combinedBackgroundFog = mix(background, waterColor, depthForFog);
	
	
	// apply
	NORMAL_MAP = combinedWaterNormal;
	ROUGHNESS = roughness;// * foamBlend;
	EMISSION = combinedBackgroundFog + foamTexture * 0.5 + waterHeight;
	ALBEDO = foamTexture;
}