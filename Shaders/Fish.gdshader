shader_type spatial;

uniform sampler2D albedo : source_color;
uniform float metallic = 0.1;
uniform float roughness = 0.5;
uniform float specular = 0.5;
uniform float waveCursor = 0.0;
uniform float waveFrequency = 0.1;
uniform float waveTurbulance = 0.1;
uniform float waveAmplitude = 0.1;

void vertex()
{
	// get world position of vertex
	vec3 worldPosition = NODE_POSITION_WORLD + VERTEX;

	// combine x and z for wave index
	float positionOffset = abs(worldPosition.x) + abs(worldPosition.z);
	positionOffset *= 0.5 * waveTurbulance;

	float xWarp = sin(waveCursor * waveFrequency + positionOffset) * waveAmplitude * COLOR.r;

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
