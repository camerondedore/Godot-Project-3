shader_type spatial;
render_mode specular_disabled, unshaded, shadows_disabled, ambient_light_disabled;

uniform vec4 albedo : source_color;
uniform float depthDistance : hint_range(0, 1) = 0.5;
uniform float depthFade : hint_range(0.001, 10) = 1;
uniform float waveturbulance : hint_range(0, 5) = 0.2;
uniform float waveSpeed : hint_range(0, 5) = 0.2;
uniform float waveAmplitude : hint_range(0, 1) = 0.2;

uniform sampler2D DEPTH_TEXTURE : hint_depth_texture;

void vertex()
{
	
}

void fragment()
{
	// get depth behind black wall
	float depthTexture = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	float depth = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[2][2]);
	float depthBlend = depth + VERTEX.z;
	
	// waves
	float waves = sin((VERTEX.x + VERTEX.z) * waveturbulance + TIME * waveSpeed) * waveAmplitude;
	
	// fog
	float fog = pow(depthBlend * (1.0 - depthDistance), depthFade);
	fog = clamp(fog, 0.0, 10.0);	
	
	// combine depth fog and waves
	float combinedEffect = fog + waves;
	
	ALPHA = combinedEffect;
	
	ALBEDO = albedo.rgb;
}