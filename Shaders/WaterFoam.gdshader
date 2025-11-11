shader_type spatial;

uniform sampler2D foamTexture1;
uniform sampler2D foamTexture2;
uniform sampler2D stochasticNoise;
uniform sampler2D stochasticNoiseSmooth;
uniform vec3 foamColor : source_color;
uniform float uvScale = 2;
uniform float foamEdgeDistance = 0.1;
uniform float foamTextureEdgeDistance = 0.5;
uniform float stochasticScale = 1.0;
uniform float stochasticMagnitude = 1.0;
uniform float warpTimeScale = 1.0;
uniform float warpSize = 0.5;
uniform float warp2Offset = 0.5;

uniform sampler2D DEPTH_TEXTURE: hint_depth_texture, filter_linear_mipmap;

void vertex()
{
	// Called for every vertex the material is visible on.
}

void fragment()
{
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec2 worldUvCoordinates = worldPosition.rb / uvScale;

	// get depth and screen textures
	float depth = textureLod(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;

	// get depth of water in world position Y depth
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	vec4 world = INV_VIEW_MATRIX * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	float depthTextureY = world.y / world.w;
	float vertexY = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).y;

	// get edge blends
	float edgeBlend = 1.0 - clamp((vertexY - depthTextureY) / foamEdgeDistance, 0.0, 1.0);
	float edgeTextureBlend = 1.0 - clamp((vertexY - depthTextureY) / foamTextureEdgeDistance, 0.0, 1.0);

	// get uv warping
	float timeCursor = TIME * warpTimeScale;
	float worldOffset = worldPosition.x + worldPosition.z;
	vec2 warpOffset1 = vec2(cos(timeCursor + worldPosition.x), sin(timeCursor + worldPosition.z))  * warpSize;
	vec2 warpOffset2 = vec2(cos(timeCursor + warp2Offset + worldPosition.x), sin(timeCursor + warp2Offset + worldPosition.z))  * warpSize;

	float stochasticOffset = texture(stochasticNoise, worldUvCoordinates.xy * stochasticScale).r * stochasticMagnitude;
	float stochasticOffsetBlend = texture(stochasticNoiseSmooth, worldUvCoordinates.xy * stochasticScale).r * stochasticMagnitude;

	// get foam textures
	float foamTex1 = texture(foamTexture1, worldUvCoordinates.xy + warpOffset1).a;
	float foamTex1Stochastic = texture(foamTexture1, worldUvCoordinates.xy + warpOffset1 + stochasticOffset).a;
	float foamTex2 = texture(foamTexture2, worldUvCoordinates.xy + warpOffset2).a;
	float foamTex2Stochastic = texture(foamTexture2, worldUvCoordinates.xy + warpOffset2 + stochasticOffset).a;
	float foamTexCombined = clamp(mix(foamTex1 + foamTex2, foamTex1Stochastic + foamTex2Stochastic, stochasticOffsetBlend), 0.0, 1.0) * edgeTextureBlend;

	// get foam edge
	float foamAlpha = clamp(foamTexCombined + edgeBlend, 0.0, 1.0);

	ALBEDO = foamColor;
	ALPHA = foamAlpha;
	ALPHA_SCISSOR_THRESHOLD = 1.0 - foamAlpha;
}
