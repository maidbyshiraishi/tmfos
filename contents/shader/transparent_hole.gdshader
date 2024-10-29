// https://www.reddit.com/r/godot/comments/zievki/help_with_transparent_wall_shader/

shader_type canvas_item;

global uniform bool TransparentEnabled;
global uniform vec2 TransparentCircleCenter;

uniform float CircleRadius = 80.0;
uniform float CircleBlur = 0.1;

void fragment() {
	if (TransparentEnabled) {
	    float circle_blur = clamp(0.0, 1.0, CircleBlur);
	    float circle_dist = distance(TransparentCircleCenter, FRAGCOORD.xy);
	    float circle_signal = 1.0 - smoothstep(CircleRadius, CircleRadius * (1.0 - circle_blur), circle_dist);
	    vec4 tex_color = texture(TEXTURE, UV);
	    COLOR = tex_color;
	    COLOR.a *= circle_signal;
	}
}
