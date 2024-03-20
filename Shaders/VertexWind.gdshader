shader_type spatial;
render_mode cull_disabled;

uniform sampler2D albedo : source_color;
uniform float backlightStrength : hint_range(0.0, 1.0) = 0.0;
uniform float alphaClip = 0.5;
uniform vec3 waveAxis = vec3(0);
uniform float waveSpeed = 1;
uniform float waveTurbulance = 1;
uniform float waveAmplitude = 0.1;

void vertex() 
{
	// get wave time
	float positionCursor = (VERTEX.x + VERTEX.y + VERTEX.z) * waveTurbulance;
	float waveCursor = sin(positionCursor + TIME * waveSpeed);
	float waveWeight = COLOR.r;
	vec3 waves = waveAxis * waveCursor * waveWeight * waveAmplitude;
	
	// apply waves
	VERTEX += waves;
}

void fragment() 
{
	// get texture data
	vec4 imageRead = texture(albedo, UV);
	
	// clip alpha
	if(imageRead.a < alphaClip)
	{
		discard;
	}
	
	ALBEDO = imageRead.rgb;
	//ALPHA = imageRead.a;
	SPECULAR = 0.0;
	METALLIC = 0.0;
	ROUGHNESS = 1.0;
	BACKLIGHT = vec3(backlightStrength);
}