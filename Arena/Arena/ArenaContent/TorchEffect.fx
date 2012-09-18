texture ScreenTexture;
float4x4 MatrixTransform : register(vs, c0);
float torch_x;
float torch_y;
float torch_str;

sampler TextureSampler = sampler_state
{
	Texture = <ScreenTexture>;
};

void SpriteVertexShader(inout float4 color : COLOR0,
						inout float2 texCoord : TEXCOORD0,
						inout float4 position : SV_Position)
{
	position = mul(position, MatrixTransform);
}

float4 PixelShaderFunction(float2 TextureCoordinate : TEXCOORD0, float2 vPos : VPOS) : COLOR0
{
	float4 color = tex2D(TextureSampler, TextureCoordinate);
	float x = vPos[0];
	float y = vPos[1];

	float value = (color.r + color.g + color.b) / 3;
	color.r = value;
	color.b = value;
	color.g = value;

	float distance = sqrt( pow( (torch_x - x), 2) + pow( (torch_y - y), 2) );

	color.a = lerp(0.0, 1.0, (distance / torch_str));

	return color;
}

technique Plain
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}