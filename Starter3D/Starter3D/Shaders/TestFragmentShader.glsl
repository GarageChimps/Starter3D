#version 330

precision highp float;

in vec3 fragPosition;
in vec3 fragNormal;
out vec4 outFragColor;

const vec3 black = vec3(0.0), 
           white = vec3(1.0);


void main(void) {
   vec3 c = (fragPosition.x > 0.0 ^^ fragPosition.y > 0.0) ? black : white;
   outFragColor = vec4(fragNormal,1.);
}

