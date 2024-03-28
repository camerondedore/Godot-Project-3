shader_type spatial;

uniform sampler2D albedo : source_color;
uniform float metallic : hint_range(0, 1) = 0;
uniform float roughness : hint_range(0, 1) = 0;
uniform float specular : hint_range(0, 1) = 0;
uniform float rimSharpness : hint_range(-1, 0.5) = 0.25;
uniform float rimDistance : hint_range(0, 2) = 1;
uniform vec4 glowColor : source_color;
uniform float glowPower = 1;

void fragment() 
{
	// get how aligned the normal is to the view
	float normalView = clamp(dot(normalize(VIEW), NORMAL), 0.0, 1.0);
	
	float rimEffect = smoothstep(rimSharpness, 1.0 - rimSharpness, rimDistance / normalView);
	rimEffect = -1.0 * clamp(rimEffect, 0.0, 1.0) + 1.0;

	vec3 rimGlow = glowColor.rgb * rimEffect * glowPower;
	
	
	ALBEDO = texture(albedo, UV).rgb;
	METALLIC = metallic;
	ROUGHNESS = roughness;
	SPECULAR = specular;
	EMISSION = rimGlow;
}