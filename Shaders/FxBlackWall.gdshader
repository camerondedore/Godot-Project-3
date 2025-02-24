shader_type spatial;
render_mode specular_disabled, unshaded, shadows_disabled, ambient_light_disabled;

uniform vec4 depthColor : source_color;
uniform float depthDistance : hint_range(0.0, 1.0) = 0.5;
uniform float depthFade : hint_range(0.001, 10) = 1;
uniform float waveTurbulance : hint_range(0.0,20.0) = 0.2;
uniform float waveSpeed : hint_range(0.0, 5.0) = 0.2;
uniform float waveAmplitude : hint_range(0.0, 1.0) = 0.2;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;

varying vec3 worldPosition;

void vertex()
{
	// get world position of vertex
	worldPosition = NODE_POSITION_WORLD + VERTEX;
}

void fragment()
{
	// get depth and screen textures
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	vec3 screenTexture = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;

	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);

	// get depth behind black wall
	vec4 viewWorld = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	viewWorld.z = viewWorld.z / viewWorld.w;
	float linearDepth = -viewWorld.z + VERTEX.z;

	// waves
	float positionWaveInput = (worldPosition.x + worldPosition.z) * waveTurbulance;
	float waves = sin(positionWaveInput + TIME * waveSpeed) + 1.0;
	//float waves = (sin((NODE_POSITION_WORLD.x + NODE_POSITION_WORLD.z) * waveturbulance + TIME * waveSpeed) + 1.0);
	waves *= waveAmplitude;
	waves = clamp(waves, 0.0, 1.0);

	// fog
	float fog = linearDepth * (1.0 - depthDistance);
	//fog = clamp(fog, 0.0, 10.0);

	// combine depth fog and waves
	float combinedEffect = fog + waves;
	combinedEffect *= linearDepth * (1.0 - depthDistance);
	combinedEffect = clamp(combinedEffect, 0.0, 1.0);

	// blend screen with combined effect
	vec3 screenWithEffect = mix(screenTexture, depthColor.rgb, combinedEffect);

	ALBEDO = screenWithEffect;
}