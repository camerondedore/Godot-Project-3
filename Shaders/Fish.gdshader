shader_type spatial;
render_mode cull_disabled;

uniform sampler2D albedo : source_color;
uniform float metallic = 0.1;
uniform float roughness = 0.5;
uniform float specular = 0.5;
uniform float waveFrequency = 0.1;
uniform float waveAmplitude = 0.1;

void vertex() 
{
	// get world position of vertex
	vec3 worldPosition = NODE_POSITION_WORLD + VERTEX;
	
	// combine x and z for wave index
	float waveIndex = abs(worldPosition.x) + abs(worldPosition.z);
	waveIndex *= 0.5;
	
	float xWarp = sin(waveIndex * waveFrequency) * waveAmplitude * COLOR.r;
	
	// apply warp
	VERTEX += vec3(xWarp, 0, 0);
}

void fragment() 
{
	ALBEDO = texture(albedo, UV).rgb;
	METALLIC = metallic;
	ROUGHNESS = roughness;
	SPECULAR = specular;
}
