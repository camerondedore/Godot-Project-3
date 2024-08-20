shader_type spatial;

uniform sampler2D albedo : source_color;
uniform float rimPower : hint_range(0, 1) = 1;
uniform float rimSharpness : hint_range(0, 1) = 0.5;
uniform float rimDistance : hint_range(0, 2) = 1;
uniform vec4 rimColor : source_color;
uniform float metallic : hint_range(0, 1) = 0;
uniform float roughness : hint_range(0, 1) = 0;
uniform float backlight : hint_range(0, 1);

void fragment()
{
	// get how aligned the normal is to the view
	float normalView = clamp(dot(normalize(VIEW), NORMAL), 0.0, 1.0);

	float rimEffect = smoothstep(0, 1.0 - rimSharpness, rimDistance - normalView);
	rimEffect = clamp(rimEffect, 0.0, 1.0);

	vec3 rimEffectColor = rimEffect * rimColor.rgb * rimPower;
	vec3 albedoColor = texture(albedo, UV).rgb * (1.0 - rimEffect * rimPower);

	ALBEDO = albedoColor + rimEffectColor;
	METALLIC = metallic;
	ROUGHNESS = roughness;
	BACKLIGHT = vec3(backlight);
}