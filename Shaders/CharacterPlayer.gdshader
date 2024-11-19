shader_type spatial;

varying float localY;

uniform sampler2D albedo : source_color;
uniform sampler2D armorColorMask : source_color;
uniform sampler2D armorOcclusionMask : source_color;
uniform vec4 armorColor : source_color;
uniform float rimPower : hint_range(0, 1) = 1;
uniform float rimPowerAlbedo : hint_range(0, 1) = 1;
uniform float rimSharpness : hint_range(0, 0.5) = 0.2;
uniform float rimDistance : hint_range(0, 2) = 1;
uniform vec4 rimColor : source_color;
uniform float rimMinHeight = 0.0;
uniform float rimHeightSlope = 1.0;

void vertex()
{
	localY = VERTEX.y;
}

void fragment()
{
	// get diffuse color
	vec3 albedoColor = texture(albedo, UV).rgb;

	// apply armor color
	float armorMask = texture(armorColorMask, UV).r;
	albedoColor = mix(albedoColor, armorColor.rgb, armorMask);

	// apply armor occlusion
	albedoColor *= texture(armorOcclusionMask, UV).r;

	// get how aligned the normal is to the view
	float normalView = clamp(dot(normalize(VIEW), NORMAL), 0.0, 1.0);

	float rimEffect = smoothstep(rimSharpness, 1.0 - rimSharpness, rimDistance - normalView);
	rimEffect = clamp(rimEffect, 0.0, 1.0);

	vec3 rimEffectColor = rimEffect * rimColor.rgb * rimPower;
	vec3 rimEffectAlbedoColor = rimEffect * albedoColor.rgb * rimPowerAlbedo;

	float rimEffectHeightMultiplier = clamp((localY - rimMinHeight) / rimHeightSlope, 0.0, 1.0);

	vec3 combinedRimEffect = (rimEffectColor + rimEffectAlbedoColor) * rimEffectHeightMultiplier;


	ALBEDO = albedoColor;
	EMISSION = combinedRimEffect;
	SPECULAR = 0.0;
	METALLIC = 0.0;
	ROUGHNESS = 1.0;
}
