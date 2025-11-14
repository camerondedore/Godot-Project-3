shader_type spatial;

uniform sampler2D albedoDefault : source_color;
uniform sampler2D albedo1 : source_color;
uniform sampler2D albedo2 : source_color;
uniform sampler2D height1 : source_color, hint_default_black;
uniform sampler2D height2 : source_color, hint_default_black;
uniform float uvScale = 1;
uniform float blendingHighSharpness : hint_range(0.0, 0.9) = 0.4;
uniform float blendingLowSharpness : hint_range(0.0, 0.9) = 0.8;

void vertex()
{

}

void fragment()
{
	// get world position and normal of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec3 worldNormal = abs(INV_VIEW_MATRIX * vec4(NORMAL, 0.0)).xyz;

	// get the axis the normal of the face is most facing
	float normalX = round(worldNormal.r);
	float normalY = round(worldNormal.g) - round(worldNormal.r) - round(worldNormal.b);
	normalY = clamp(normalY, 0.0, 1.0);
	float normalZ = round(worldNormal.b) - round(worldNormal.r);
	normalZ = clamp(normalZ, 0.0, 1.0);

	// get uvs for each axis
	vec2 uvX = worldPosition.zy;
	vec2 uvY = worldPosition.xz;
	vec2 uvZ = worldPosition.xy;

	// pick uv direction to use
	vec2 uvToUse = uvX * normalX + uvY * normalY + uvZ * normalZ;
	uvToUse *= uvScale;

	// get textures, uses world coordinates
	vec3 defaultTexture = texture(albedoDefault, uvToUse).rgb;
	vec3 texture1 = texture(albedo1, uvToUse).rgb;
	vec3 texture2 = texture(albedo2, uvToUse).rgb;

	// get textures, uses world coordinates
	float heightValue1 = texture(height1, uvToUse).r;
	float heightValue2 = texture(height2, uvToUse).r;

	// 1st step gets pixels from albedo using hightmap
	// 2nd step combines the hightmap pixels and the regular albedo from the vertex colors
	float blend1 = ceil(smoothstep(blendingHighSharpness, 1.0, COLOR.r * heightValue1));
	blend1 = clamp(smoothstep(blendingLowSharpness, 1.0, COLOR.r) + blend1, 0.0, 1.0);
	float blend2 = ceil(smoothstep(blendingHighSharpness, 1.0, COLOR.g * heightValue2));
	blend2 = clamp(smoothstep(blendingLowSharpness, 1.0, COLOR.g) + blend2, 0.0, 1.0);

	// finally combine textures from bottom to top
	vec3 mixedTextures = mix(defaultTexture, texture1, blend1);
	mixedTextures = mix(mixedTextures, texture2, blend2);


	//ALBEDO = vec3(blend1, blend2, blend3);
	//ALBEDO = COLOR.rgb;
	//ALBEDO = vec3(normalX, normalY, normalZ);
	ALBEDO = mixedTextures;
	METALLIC = 0.0;
	ROUGHNESS = 1.0;
	SPECULAR = 0.0;
}
