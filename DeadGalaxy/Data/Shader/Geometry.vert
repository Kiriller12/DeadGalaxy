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

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;
in vec4 vertexTangent;

// Input uniform values
uniform mat4 mvp;
uniform mat4 matModel;
uniform mat4 matNormal;

// Output vertex attributes (to fragment shader)
out vec2 fragTexCoord;
out vec4 fragColor;
out vec3 fragNormal;
out vec3 fragTangent;
out vec3 fragBinormal;
out vec3 fragPosition;

void main()
{
    // Calculating vertex attributes for fragment shader
    fragTexCoord = vertexTexCoord;
    fragColor = vertexColor;
    fragNormal = normalize(vec3(matNormal * vec4(vertexNormal, 1.0)));
    fragTangent = normalize(vec3(matNormal * vec4(vertexTangent.xyz, 1.0)));
    fragBinormal = normalize(cross(fragNormal, fragTangent) * vertexTangent.w);
    fragPosition = (matModel * vec4(vertexPosition, 1.0)).xyz;

    // Calculating final vertex position
    gl_Position = mvp * vec4(vertexPosition, 1.0);
}