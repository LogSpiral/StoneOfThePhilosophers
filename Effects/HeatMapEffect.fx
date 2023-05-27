//sampler uImage0 : register(s0); //底层静态图
//sampler uImage1 : register(s1); //偏移灰度图
//sampler uImage2 : register(s2); //采样/着色图
//float4x4 uTransform;
//float uTimeX;
//float uTimeY;

//struct VSInput
//{
//	float2 Pos : POSITION0;
//	float4 Color : COLOR0;
//	float3 Texcoord : TEXCOORD0;
//};
//struct PSInput
//{
//	float4 Pos : SV_POSITION;
//	float4 Color : COLOR0;
//	float3 Texcoord : TEXCOORD0;
//};
//PSInput VertexShaderFunction(VSInput input)
//{
//	PSInput output;
//	output.Color = input.Color;
//	output.Texcoord = input.Texcoord;
//	output.Pos = float4(input.Pos, 0, 1);
//	return output;
//}
//float GetGrayValue(float3 color)
//{
//	float max = color.x;
//	float min = color.y;
//	if (min > max)
//	{
//		max = min;
//		min = color.x;
//	}
//	if (color.z > max)
//	{
//		max = color.z;
//	}
//	if (color.z < min)
//	{
//		min = color.z;
//	}
//	return (max + min) * 0.5;
//}
//float4 GetGrayVector(float2 coord)
//{
//	return tex2D(uImage0, coord) * tex2D(uImage1, coord + float2(uTimeX, uTimeY));
//}
//float Gray(float2 coord)
//{
//	return GetGrayValue(GetGrayVector(coord).xyz);
//}
//float4 FinalModify(float4 color, float k)
//{
//	if (!any(color))
//		return float4(0, 0, 0, 0);
//	if (k < 0.5)
//	{
//		return float4(lerp(float3(0, 0, 0), color.xyz, 2 * k), color.w);
//	}
//	return float4(lerp(color.xyz, float3(1, 1, 1), 2 * k - 1), color.w);
//}
//float4 PixelShaderFunction_HeatMap(PSInput input) : COLOR0
//{
//	float4 grayVector = GetGrayVector(input.Texcoord.xy);
//	float4 color = tex2D(uImage2, grayVector.xy + float2(uTimeX, uTimeY));
//	return float4(FinalModify(color, grayVector.x).xyz, input.Color.w);
//}
//technique Technique1
//{
//	pass HeatMap
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction_HeatMap();
//	}
//}
sampler uImage0 : register(s0); //底层静态图
sampler uImage1 : register(s1); //偏移灰度图
sampler uImage2 : register(s2); //采样/着色图
float4x4 uTransform;
float2 uTime;
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
	return tex2D(uImage0, coord) * tex2D(uImage1, coord + uTime);
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
float4 PixelShaderFunction_HeatMap(PSInput input) : COLOR0
{
	float2 texcoord = mul(float4(input.Texcoord.xy, 0, 1), uTransform);
	float4 color = tex2D(uImage2, float2(Gray(texcoord), 1));
	return float4(color.xyz, input.Color.w * tex2D(uImage0, texcoord).w);
}
float4 PixelShaderFunction_HeatMap2(PSInput input) : COLOR0
{
	float2 texcoord = mul(float4(input.Texcoord.xy, 0, 1), uTransform);
	float4 grayVector = GetGrayVector(texcoord);
	float4 color = tex2D(uImage2, float2(GetGrayValue(grayVector.xyz), 1));
	return float4(FinalModify(color, grayVector.x).xyz, input.Color.w * tex2D(uImage0, texcoord).w);
}
technique Technique1
{
	pass HeatMap
	{
		PixelShader = compile ps_2_0 PixelShaderFunction_HeatMap();
	}
	pass HeatMap2
	{
		PixelShader = compile ps_2_0 PixelShaderFunction_HeatMap2();
	}
}