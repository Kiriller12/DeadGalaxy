/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

========================================================= 
*/

#version 330

// Ambient light intensity
const float AmbientLight = 0.1;

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;

// Input uniform values
uniform sampler2D diffuseTexture;
uniform sampler2D lightingTexture;

// Output fragment values
out vec4 finalColor;

void main()
{
    vec4 diffuse = texture(diffuseTexture, fragTexCoord);
    vec3 lighting = texture(lightingTexture, fragTexCoord).rgb;

    finalColor = diffuse * AmbientLight + vec4(lighting.xyz, 1.0);
}