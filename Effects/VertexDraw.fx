sampler uImage0 : register(s0); //底层静态图
sampler uImage1 : register(s1); //偏移灰度图
sampler uImage2 : register(s2); //采样/着色图
float4x4 uTransform;
float uTimeX;
float uTimeY;
struct VSInput
{
	float2 Pos : POSITION0;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};
struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};
float GetGrayValue(float3 color)
{
	float max = color.x;
	float min = color.y;
	if (min > max)
	{
		max = min;
		min = color.x;
	}
	if (color.z > max)
	{
		max = color.z;
	}
	if (color.z < min)
	{
		min = color.z;
	}
	return (max + min) * 0.5;
}
float4 GetGrayVector(float2 coord)
{
	return tex2D(uImage0, coord) * tex2D(uImage1, coord + float2(uTimeX, uTimeY));
}
float Gray(float2 coord)
{
	return GetGrayValue(GetGrayVector(coord).xyz);
}
float4 FinalModify(float4 color, float k)
{
	if (!any(color))
		return float4(0, 0, 0, 0);
	if (k < 0.5)
	{
		return float4(lerp(float3(0, 0, 0), color.xyz, 2 * k), color.w);
	}
	return float4(lerp(color.xyz, float3(1, 1, 1), 2 * k - 1), color.w);
}

float4 PixelShaderFunction_Color(PSInput input) : COLOR0
{
	float4 color = input.Color * input.Texcoord.z;
	return FinalModify(color, Gray(input.Texcoord.xy));
}
float4 PixelShaderFunction_HeatMap(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage2, float2(GetGrayVector(input.Texcoord.xy).xy));
	return FinalModify(color, input.Texcoord.z);
}
float4 PixelShaderFunction_ColorMap(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage2, input.Texcoord.xy) * input.Texcoord.z;
	return FinalModify(color, Gray(input.Texcoord.xy));
}
PSInput VertexShaderFunction(VSInput input)
{
	PSInput output;
	output.Color = input.Color;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
	return output;
}


technique Technique1
{
	pass Color
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction_Color();
	}
	pass HeatMap
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction_HeatMap();
	}
	pass ColorMap
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction_ColorMap();
	}
}