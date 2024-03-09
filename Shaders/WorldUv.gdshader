shader_type spatial;

uniform sampler2D albedo : source_color;
uniform float uvScale = 5;

void vertex() 
{
	
}

void fragment() 
{
	// get world position of pixel
	vec3 worldPosition = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;
	//vec2 worldUvCoordinates = (worldPosition.rb) / uvScale;
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
	
	//ALBEDO = vec3(normalX, normalY, normalZ);
	ALBEDO = texture(albedo, uvToUse).xyz;
	ROUGHNESS = 1.0;
	METALLIC = 0.0;
	SPECULAR = 0.0;
}