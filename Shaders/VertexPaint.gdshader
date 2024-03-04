shader_type spatial;

uniform sampler2D albedoDefault : source_color;
uniform sampler2D albedo1 : source_color;
uniform sampler2D albedo2 : source_color;
uniform sampler2D albedo3 : source_color;
uniform sampler2D height1 : source_color;
uniform sampler2D height2 : source_color;
uniform sampler2D height3 : source_color;
//uniform float uvScale = 5;
uniform float blendingHighSharpness : hint_range(0.0, 0.9) = 0.4;
uniform float blendingLowSharpness : hint_range(0.0, 0.9) = 0.8;

void vertex() 
{
	
}

void fragment() 
{
	// get world position of pixel
	//vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	//vec2 worldUvCoordinates = (worldPosition.rb + worldPosition.g) / uvScale;
	
	// get textures, uses world coordinates
	vec3 defaultTexture = texture(albedoDefault, UV).rgb;
	vec3 texture1 = texture(albedo1, UV).rgb;
	vec3 texture2 = texture(albedo2, UV).rgb;
	vec3 texture3 = texture(albedo3, UV).rgb;
	
	// get textures, uses world coordinates
	float heightValue1 = texture(height1, UV).r;	
	float heightValue2 = texture(height2, UV).r;
	float heightValue3 = texture(height3, UV).r;
	
	// 1st step gets pixels from albedo using hightmap
	// 2nd step combines the hightmap pixels and the regular albedo from the vertex colors
	float blend1 = ceil(smoothstep(blendingHighSharpness, 1.0, COLOR.r * heightValue1));
	blend1 = clamp(smoothstep(blendingLowSharpness, 1.0, COLOR.r) + blend1, 0.0, 1.0);
	float blend2 = ceil(smoothstep(blendingHighSharpness, 1.0, COLOR.g * heightValue2));
	blend2 = clamp(smoothstep(blendingLowSharpness, 1.0, COLOR.g) + blend2, 0.0, 1.0);
	float blend3 = ceil(smoothstep(blendingHighSharpness, 1.0, COLOR.b * heightValue3));
	blend3 = clamp(smoothstep(blendingLowSharpness, 1.0, COLOR.b) + blend3, 0.0, 1.0);
	
	// finally combine textures from bottom to top
	vec3 mixedTextures = mix(defaultTexture, texture1, blend1);
	mixedTextures = mix(mixedTextures, texture2, blend2);
	mixedTextures = mix(mixedTextures, texture3, blend3);
	

	//ALBEDO = vec3(blend1, blend2, blend3);
	//ALBEDO = COLOR.rgb;
	ALBEDO = mixedTextures;
	METALLIC = 0.0;
	ROUGHNESS = 0.95;
	SPECULAR = 0.25;
}
