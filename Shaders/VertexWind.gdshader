shader_type spatial;
render_mode cull_disabled;

uniform sampler2D albedo : source_color;
uniform float backlightStrength : hint_range(0.0, 1.0) = 0.0;
uniform float alphaClip = 0.5;
uniform vec3 waveSpeed = vec3(1);
uniform float waveTurbulance = 1;
uniform vec3 waveAmplitude = vec3(0.1);

void vertex() 
{
	// get wave time
	float positionCursor = (VERTEX.x + VERTEX.y + VERTEX.z) * waveTurbulance;
	float waveCursorX = sin(positionCursor + TIME * waveSpeed.x);
	float waveCursorY = sin(positionCursor + TIME * waveSpeed.y);
	float waveCursorZ = sin(positionCursor + TIME * waveSpeed.z);
	float waveWeight = COLOR.r;
	float wavesX = waveCursorX * waveAmplitude.x;
	float wavesY = waveCursorY * waveAmplitude.y;
	float wavesZ = waveCursorZ * waveAmplitude.z;
	vec3 waves = vec3(wavesX, wavesY, wavesZ) * waveWeight;
	
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