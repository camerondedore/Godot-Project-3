shader_type spatial;
render_mode cull_disabled;

uniform float waveFrequency = 0.1;
uniform float waveAmplitude = 0.1;

void vertex() 
{
	// get world position of vertex
	vec3 worldPosition = NODE_POSITION_WORLD + VERTEX;
	
	// combine x and z for wave index
	float waveIndex = abs(worldPosition.x) + abs(worldPosition.z);
	waveIndex *= 0.5;
	
	float xWarp = sin(waveIndex * waveFrequency) * waveAmplitude * COLOR.r;
	
	// apply warp
	VERTEX += vec3(xWarp, 0, 0);
}

void fragment() 
{
	// Called for every pixel the material is visible on.
}
