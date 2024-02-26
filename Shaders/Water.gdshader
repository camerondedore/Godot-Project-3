shader_type spatial;

uniform vec3 waterColor : source_color;
uniform vec3 foamColor : source_color;
uniform float roughness = 1;
uniform float uvScale = 5;
uniform float waveTimeScale : hint_range(0.0, 1.0) = 0.25;
uniform vec2 waveVelocity1 = vec2(0.1, 0.2);
uniform vec2 waveVelocity2 = vec2(-0.2, -0.1);
uniform float refractionStrengthXY : hint_range(0.0, 1.0) = 0.05;
uniform float refractionStrengthZ : hint_range(0.0, 1.0) = 0.025;
uniform float depthOffset = 1;
uniform float beersLaw = 1;
uniform sampler2D waterNormalMap1;
uniform sampler2D waterNormalMap2;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture, filter_linear_mipmap;


void vertex() 
{
	
}

void fragment() 
{
	float depthTexture = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	float depth = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[2][2]);
	float depthBlend = depth + VERTEX.z;
	
	// foam
	vec3 foam = foamColor * clamp(pow(1.0 - depthBlend + 0.2, 10), 0.0, 1.0);;
		
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec2 worldUvCoordinates = worldPosition.rb / uvScale;
	
	// get time offsets for waves
	vec2 waveOffset1 = TIME * waveVelocity1 * waveTimeScale;
	vec2 waveOffset2 = TIME * waveVelocity2 * waveTimeScale;
	
	// get normal map from texture using world position for UV coordinated
	vec3 waterNormal1 = texture(waterNormalMap1, worldUvCoordinates + waveOffset1).rgb;
	vec3 waterNormal2 = texture(waterNormalMap2, worldUvCoordinates + waveOffset2).rgb;
	vec3 combinedWaterNormal = mix(waterNormal1, waterNormal2, 0.5);
	NORMAL_MAP = combinedWaterNormal;
	
	
	// refraction
	vec3 unpacked_normal = NORMAL_MAP;
	unpacked_normal.xy = unpacked_normal.xy * 2.0 - 1.0;
	unpacked_normal.z = sqrt(max(0.0, 1.0 - dot(unpacked_normal.xy, unpacked_normal.xy)));
	vec3 ref_normal = normalize( mix(NORMAL,TANGENT * unpacked_normal.x + BINORMAL * unpacked_normal.y + NORMAL * unpacked_normal.z * refractionStrengthZ,NORMAL_MAP_DEPTH) );
	vec2 ref_ofs = SCREEN_UV - ref_normal.xy * refractionStrengthXY * depthBlend;
	
	// apply
	ROUGHNESS = roughness;
	ALPHA = 1.0;
	EMISSION = texture(SCREEN_TEXTURE, ref_ofs).rgb + foam * foamColor;
	ALBEDO = vec3(0,0,0);
	//ALBEDO = waterColor * depthBlendPower;
}