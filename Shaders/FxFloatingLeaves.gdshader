shader_type spatial;

uniform sampler2D albedo : source_color;
uniform vec3 wetColor : source_color;
uniform sampler2D normal : hint_normal;
uniform float normalStrength = 1.0;
uniform float roughness = 0.5;
uniform float specular = 0.5;
uniform float metallic = 0.5;
uniform float alphaCuttoff = 0.5;
uniform float waveMagnitude = 0.1;
uniform float waveFrequency = 1.0;
uniform float waveSpeed = 1.0;

void fragment() 
{
	// get world position
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	
	float waveX = sin(TIME * waveSpeed + worldPosition.x * waveFrequency) * waveMagnitude;
	float waveZ = cos(TIME * waveSpeed + worldPosition.z * waveFrequency) * waveMagnitude;
	
	vec2 wavedUV = vec2(clamp(waveX + UV.x, 0.0, 1.0), clamp(waveZ + UV.y, 0.0, 1.0));
	
	ALBEDO = texture(albedo, wavedUV).rgb * wetColor;
	NORMAL_MAP = mix(vec3(0.5, 0.5, 1.0), texture(normal, wavedUV).rgb, normalStrength);
	ROUGHNESS = roughness;
	SPECULAR = specular;
	METALLIC = metallic;
	ALPHA = texture(albedo, wavedUV).a;
	ALPHA_SCISSOR_THRESHOLD = alphaCuttoff;
}