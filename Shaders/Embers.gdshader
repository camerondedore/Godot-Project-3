shader_type spatial;

uniform sampler2D albedo : source_color;
uniform sampler2D emission : source_color;
uniform float emissionStrength = 3;
uniform float emissionSpread = 0.5;
uniform float speed = 3;
uniform float turbulance = 3.0;

varying vec3 worldPosition;

void vertex()
{
	// get world position of vertex
	worldPosition = NODE_POSITION_WORLD + VERTEX;
}

void fragment()
{
	float combinedPosition = worldPosition.x + worldPosition.y + worldPosition.z;
	combinedPosition *= turbulance;
	float emissionAnimated = sin(combinedPosition + TIME * speed) * emissionSpread + 1.0;

	vec3 emissionColor = texture(emission, UV).xyz;
	emissionColor *= emissionStrength * emissionAnimated;

	ALBEDO = texture(albedo, UV).xyz;
	EMISSION = emissionColor;
}