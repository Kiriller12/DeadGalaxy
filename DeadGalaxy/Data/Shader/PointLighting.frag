/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

========================================================= 
*/

#version 330

const float FarClip = 100.0;
const int MaxLights = 10;

// Light data struct
struct Light
{
    vec3 position;
    float radius;
    float intensity;
    vec3 color;
};

// Input vertex attributes (from vertex shader)
in vec3 fragPosition;

// Input uniform values
uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;
uniform sampler2D specularTexture;
uniform sampler2D depthTexture;

uniform vec3 viewPos;
uniform vec2 screenSize;
uniform int lightsCount;

uniform Light[MaxLights] lights;

// Output fragment values
out vec4 finalColor;

// Calculates world position from depth
vec3 getWorldPosition(float depth)
{
	vec3 direction = normalize(fragPosition - viewPos);
	
	return viewPos + (direction * depth * FarClip);
}

// Calculates light attenuation
float getLightAttenuation(float distanceToPoint, float lightRadius, float intensity, float fallOff)
{
	float s = distanceToPoint / lightRadius;
	if (s >= 1.0)
    {
		return 0.0;
    }

	float s2 = s * s;

	return intensity * ((1.0 - s2) * (1.0 - s2)) / (1.0 + fallOff * s2);
}

void main()
{
    // Calculating texture coordinates in screen space
    vec2 fragCoord = gl_FragCoord.xy / screenSize;

    vec3 diffuse = texture(diffuseTexture, fragCoord).rgb;
    vec3 normal = texture(normalTexture, fragCoord).rgb * 2.0 - 1.0; // Converting data from [0; 1] to [-1; 1] format
    vec3 specular = texture(specularTexture, fragCoord).rgb;
    float depth = texture(depthTexture, fragCoord).r;

    vec3 pixelPosition = getWorldPosition(depth);
    vec3 viewDir = normalize(viewPos - pixelPosition);

    // Accumulating light for current fragment
    vec3 lighting = vec3(0.0);
    for (int i = 0; i < lightsCount; i++)
    {
        // If light radius is too small, skipping
        if(length(lights[i].radius) < 0.001)
        {
            continue;
        }

        // If light source affects the fragment
        float dist = length(lights[i].position - pixelPosition);
        if(dist < lights[i].radius)
        {
            // Calculating light attenuation
            float attenuation = getLightAttenuation(dist, lights[i].radius, lights[i].intensity, 1.0);

            // If light attenuation is too small, skipping
            if(attenuation < 0.001)
            {
                continue;
            }

            // Calculating diffuse lighting
            vec3 lightDir = normalize(lights[i].position - pixelPosition);
            float lightValue = max(dot(normal, lightDir), 0.0);

            // If light value is too small, skipping
            if(lightValue < 0.001)
            {
                continue;
            }

            vec3 diffuseLight = lightValue * diffuse;

            // Calculating specular lighting
            vec3 halfwayDir = normalize(lightDir + viewDir); 
            vec3 specularLight = pow(max(dot(normal, halfwayDir), 0.0), 16.0) * specular;

            // Accumulating light
            lighting += (diffuseLight + specularLight) * lights[i].color * attenuation;
        }
    }

    finalColor = vec4(lighting, 1.0);
}