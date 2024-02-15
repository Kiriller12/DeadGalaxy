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

// Far clip plane
const float FarClip = 100.0;

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;
in vec3 fragNormal;
in vec3 fragTangent;
in vec3 fragBinormal;
in vec3 fragPosition;

// Input uniform values
uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;
uniform sampler2D specularTexture;

uniform vec3 viewPos;

// Output fragment values
out vec4 diffuse;
out vec3 normal;
out vec3 specular;
out float depth;

void main()
{
    diffuse = texture(diffuseTexture, fragTexCoord);

    mat3 tbn = transpose(mat3(fragTangent, fragBinormal, fragNormal));
    vec3 normalMap = normalize(texture(normalTexture, fragTexCoord).rgb * 2.0 - 1.0); // Converting data from [0; 1] to [-1; 1] format
    normal = normalize(normalMap * tbn) * 0.5 + 0.5; // Converting data from [-1; 1] to [0; 1] color format

    specular = texture(specularTexture, fragTexCoord).rgb;
    depth = length(fragPosition - viewPos) / FarClip;
}