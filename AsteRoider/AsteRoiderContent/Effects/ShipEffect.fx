float4x4 World;
float4x4 View;
float4x4 Projection;
float3 lightDirection;
float4 lightColor;
float lightBrightness;


float4 ambientLightColor;
float ambientLightLevel;

// TODO: add effect parameters here.
texture shipTexture1;

sampler2D textureSampler = sampler_state {
	Texture = (shipTexture1);

	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : NORMAL0;
	// TODO: add input channels such as texture
	// coordinates and vertex colors here.
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 LightingColor : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;

	// TODO: add vertex shader outputs such as colors and texture
	// coordinates here. These values will automatically be interpolated
	// over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.TextureCoordinate = input.TextureCoordinate;

	float4 normal = normalize(mul(input.Normal, World));
	float lightLevel = dot(normal, lightDirection);
	output.LightingColor = saturate(lightColor * lightBrightness * lightLevel);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 pixelColor = tex2D(
	textureSampler, input.TextureCoordinate);
	pixelColor *= input.LightingColor;
	pixelColor += ambientLightColor * ambientLightLevel;
	pixelColor.a = 1.0;
	return pixelColor;
}

float4 PixelShaderCircle(VertexShaderOutput input) : COLOR0
{
	//float4 pixelColor = COLOR_WHITE;
	//return float4(0.11,011,0.90,0.5);
	return tex2D(textureSampler, input.TextureCoordinate);
}

technique MegaRenderManiacStreetStyle
{
	pass Pass1
	{
		// TODO: set renderstates here.

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	/*pass Circle
	{
		PixelShader = compile ps_2_0 PixelShaderCircle();
		
	}*/
}
