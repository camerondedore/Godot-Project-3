shader_type spatial;
render_mode specular_disabled, unshaded, shadows_disabled, ambient_light_disabled;

uniform vec4 depthColor : source_color;
uniform float depthDistance : hint_range(-1, 1) = 0.5;
uniform float depthFade : hint_range(0.001, 10) = 1;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;



void fragment()
{
	// get depth behind black wall
	float depthTexture = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	float depth = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[2][2]);
	float depthBlend = depth + VERTEX.z;
	
	// get screen texture
	vec3 screen = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;
	
	// fog
	float fog = pow(depthBlend * (1.0 - depthDistance), depthFade);
	
	//fog *= depthBlend;
	fog = clamp(fog, 0.0, 1.0);
	
	// blend screen with combined effect
	vec3 screenWithEffect = mix(screen, depthColor.rgb, fog);
	
	ALBEDO = screenWithEffect;
}