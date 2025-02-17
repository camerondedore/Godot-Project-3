shader_type spatial;
render_mode unshaded, blend_sub;

uniform float depthDistance = 4.0;
uniform vec3 waterAbsorbColor : source_color = vec3(1.0, 0.35, 0.0);

//uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture, filter_linear_mipmap;

void fragment()
{
	// get depth and screen textures
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV).r;
	//float refractedDepth = texture(DEPTH_TEXTURE, refractedUv).r;
	//vec3 screenTexture = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;

	// get depth of water in world position Y depth
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	vec4 world = INV_VIEW_MATRIX * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	float depthTextureY = world.y / world.w;
	float vertexY = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).y;

	// get blend
	float depthBlend = clamp((vertexY - depthTextureY) / depthDistance, 0.0, 1.0);

	// get background color
	//vec3 background = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0).rgb;
	// apply color absorbtion to background
	//background = clamp(background - waterAbsorbColor * depthBlendColorAbsorbtion, 0.0, 1.0);

	ALBEDO = waterAbsorbColor * depthBlend;
}