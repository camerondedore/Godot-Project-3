shader_type spatial;

uniform sampler2D albedoDefault : source_color;
uniform sampler2D albedo1 : source_color;
uniform sampler2D albedo2 : source_color;
uniform sampler2D albedo3 : source_color;
uniform sampler2D height1 : source_color;
uniform sampler2D height2 : source_color;
uniform sampler2D height3 : source_color;
uniform float uvScale = 5;
uniform float blendingSharpness : hint_range(1.0, 5.0) = 1;

void vertex() 
{
	
}

void fragment() 
{
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec2 worldUvCoordinates = (worldPosition.rb + worldPosition.g) / uvScale;
	
	// get textures, uses world coordinates
	vec3 defaultTexture = texture(albedoDefault, worldUvCoordinates).rgb;
	vec3 texture1 = texture(albedo1, worldUvCoordinates).rgb;
	vec3 texture2 = texture(albedo2, worldUvCoordinates).rgb;
	vec3 texture3 = texture(albedo3, worldUvCoordinates).rgb;
	
	// get textures, uses world coordinates
	float heightDefaultValue = 0.0;
	float heightValue1 = texture(height1, worldUvCoordinates).r;	
	float heightValue2 = texture(height2, worldUvCoordinates).r;
	float heightValue3 = texture(height3, worldUvCoordinates).r;
	
	//float blend1 =  COLOR.r * (1.0 + heightValue1);
	//blend1 = round(smoothstep(blendingSharpness, 1.0, blend1));
	//float blend2 =  COLOR.g * (1.0 + heightValue2);
	//blend2 = round(smoothstep(blendingSharpness, 1.0, blend2));
	//float blend3 =  COLOR.b * (1.0 + heightValue3);
	//blend3 = round(smoothstep(blendingSharpness, 1.0, blend3));
	
	float blend1 = clamp(COLOR.r * blendingSharpness, 0.0, 1.0);
	float blend2 = clamp(COLOR.g * blendingSharpness, 0.0, 1.0);
	float blend3 = clamp(COLOR.b * blendingSharpness, 0.0, 1.0);
	
	float mixedBlends = blend1 + blend2 + blend3;
	
	vec3 mixedTextures = mix(defaultTexture, texture1, blend1);
	mixedTextures = mix(mixedTextures, texture2, clamp(blend2, 0.0, 1.0));
	mixedTextures = mix(mixedTextures, texture3, clamp(blend3, 0.0, 1.0));
	

	//ALBEDO = vec3(blend1, blend2, blend3);
	//ALBEDO = COLOR.rgb;
	ALBEDO = mixedTextures;
	METALLIC = 0.0;
	ROUGHNESS = 0.95;
	SPECULAR = 0.25;
	EMISSION = vec3(0.0);
}
