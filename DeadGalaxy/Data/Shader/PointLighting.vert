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

// Input vertex attributes
in vec3 vertexPosition;

// Input uniform values
uniform mat4 mvp;
uniform mat4 matModel;

// Output vertex attributes (to fragment shader)
out vec3 fragPosition;

void main()
{
    // Send vertex attributes to fragment shader
    fragPosition = (matModel * vec4(vertexPosition, 1.0)).xyz;

    // Calculate final vertex position
    gl_Position = mvp * vec4(vertexPosition, 1.0);
}