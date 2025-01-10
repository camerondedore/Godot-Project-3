shader_type spatial;
render_mode specular_disabled, unshaded, shadows_disabled, ambient_light_disabled;

uniform vec4 depthColor : source_color;
uniform float depthDistance : hint_range(-1, 1) = 0.5;
uniform float depthFade : hint_range(0.001, 10) = 1;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;



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

	// fog
	float fog = pow(linearDepth * (1.0 - depthDistance), depthFade);

	//fog *= depthBlend;
	fog = clamp(fog, 0.0, 1.0);

	// blend screen with combined effect
	vec3 screenWithEffect = mix(screenTexture, depthColor.rgb, fog);

	ALBEDO = screenWithEffect;
}