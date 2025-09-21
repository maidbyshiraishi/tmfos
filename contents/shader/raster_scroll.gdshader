// https://qiita.com/cabbagestole/items/e864bc21b4b66c0c6f41

shader_type canvas_item;

uniform float speed = 0.5;
uniform float depth = 0.1;

void fragment() {
	vec2 vec = UV;
	float t = mod(TIME * speed, 1);
	float radian = (vec.y - t) * PI * 2.0;
	vec.x = vec.x + sin(radian) * depth;
	COLOR = texture(TEXTURE, vec);
}