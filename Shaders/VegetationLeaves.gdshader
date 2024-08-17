shader_type spatial;
render_mode cull_disabled;

uniform float windAmplitude = 0.1;
uniform float windSpeed = 0.5;
uniform float windTurbulance = 0.5;
uniform float windMinHeight = 1;
uniform float windSlope = 1;
uniform float yMultiplier = 0.25;
uniform sampler2D albedo : source_color;
uniform float metallic = 0.5;
uniform float roughness = 1;
uniform float specular = 0.25;
uniform float backlightStrength = 0;
uniform float alphaScissor = 0.5;

void vertex()
{
	// get world position of vertex
	vec3 worldPosition = NODE_POSITION_WORLD + VERTEX;

	float windX = sin(worldPosition.x * windTurbulance + TIME * windSpeed);
	float windY = sin(worldPosition.y * windTurbulance + TIME * windSpeed) * yMultiplier;
	float windZ = sin(worldPosition.z * windTurbulance + TIME * windSpeed);

	float windYWeight = (VERTEX.y - windMinHeight) * windSlope;
	windYWeight = clamp(windYWeight, 0.0, 1.0);

	vec3 windEffect = vec3(windX, windY, windZ) * windAmplitude * windYWeight;

	VERTEX += windEffect;
}

void fragment()
{
	vec4 imageRead = texture(albedo, UV);

	ALBEDO = imageRead.xyz;
	ALPHA = imageRead.a;
	ALPHA_SCISSOR_THRESHOLD = alphaScissor;
	METALLIC = metallic;
	SPECULAR = specular;
	ROUGHNESS = roughness;
	BACKLIGHT = vec3(backlightStrength);
}