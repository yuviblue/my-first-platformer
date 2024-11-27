shader_type canvas_item;

void fragment() {
	vec2 pos = UV;
	pos.y = pos.y + TIME * -0.20;
	COLOR = texture(TEXTURE, pos);
}