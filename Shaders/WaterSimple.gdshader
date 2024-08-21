shader_type spatial;

uniform vec3 waterColor : source_color;
uniform vec3 foamColor : source_color;
uniform float foamPow = 10;
uniform float emissionStrength : hint_range(0.0, 1.0) = 0.5;

uniform float uvScale = 5;
uniform float timeScale = 0.25;
uniform vec2 waterVelocity1 = vec2(0.1, 0.2);
uniform vec2 waterVelocity2 = vec2(-0.2, -0.1);

uniform sampler2D waterTexture1;
uniform sampler2D waterTexture2;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;

//varying vex3 worldPosition;

void vertex()
{
	// get world position of vertex
	//worldPosition = NODE_POSITION_WORLD + VERTEX;
}


void fragment()
{
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec2 worldUvCoordinates = worldPosition.rb / uvScale;

	// get depth
	float depthTexture = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	float depth = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[2][2]);
	float depthBlend = depth + VERTEX.z;

	// get foam
	float edgeFoamValue = clamp(pow(1.0 - depthBlend + 0.2, foamPow), 0.0, 1.0);
	vec3 edgeFoam = foamColor * edgeFoamValue;

	// get time offsets for waters
	vec2 waterOffset1 = TIME * waterVelocity1 * timeScale;
	vec2 waterOffset2 = TIME * waterVelocity2 * timeScale;

	// get water texture
	float foamWaves1 = texture(waterTexture1, worldUvCoordinates.xy + waterOffset1).r;
	float foamWaves2 = texture(waterTexture2, worldUvCoordinates.xy + waterOffset2).r;
	vec3 foamWaves = foamColor * pow(foamWaves1 * foamWaves2, foamPow);

	// apply
	ALBEDO = foamWaves + mix(waterColor, edgeFoam, edgeFoamValue);
	EMISSION = mix(foamWaves, edgeFoam, edgeFoamValue) * emissionStrength;
	ROUGHNESS = 0.05;
}