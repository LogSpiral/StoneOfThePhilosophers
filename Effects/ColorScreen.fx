//sampler uImage0 : register(s0);
//sampler uImage1 : register(s1);
//sampler uImage2 : register(s2);
//sampler uImage3 : register(s3);
//float3 uColor;
//float3 uSecondaryColor;
//float2 uScreenResolution;
//float2 uScreenPosition;
//float2 uTargetPosition;
//float2 uDirection;
//float uOpacity;
//float uTime;
//float uIntensity;
//float uProgress;
//float2 uImageSize1;
//float2 uImageSize2;
//float2 uImageSize3;
//float2 uImageOffset;
//float uSaturation;
//float4 uSourceRect;
//float2 uZoom;
//float4 mainColor;

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
//float4 PixelShaderFunction(PSInput input) : COLOR0
//{
//	float4 result = tex2D(uImage0, input.Texcoord.xy);
//	return dot(result, mainColor) / dot(mainColor, mainColor) * mainColor;
//}
//float4 PixelShaderFunctionMagnifier(PSInput input) : COLOR0
//{
//	float4 color = tex2D(uImage0, input.Texcoord.xy);
//	if (!any(color))
//		return color;
//    // pos 就是中心了
//	float2 pos = float2(0.5, 0.5);
//    // offset 是中心到当前点的向量
//	float2 offset = (input.Texcoord.xy - pos);
//    // 因为长宽比不同进行修正
//	float2 rpos = offset * float2(uScreenResolution.x / uScreenResolution.y, 1);
//	float dis = length(rpos);
//    // 向量长度缩短0.8倍
//	return tex2D(uImage0, pos + offset * 0.8);
//}
//float4 PixelShaderFunctionDarkAndFlip(PSInput input) : COLOR0
//{
//	return tex2D(uImage0, float2(1, 1) - input.Texcoord.xy) * 0.5;
//}

//technique Technique1
//{
//	pass ColorScreen
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
//	}
//	pass Magnifier
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunctionMagnifier();
//	}
//	pass DarkAndFlip
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunctionDarkAndFlip();
//	}
//}
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float4 mainColor;

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};
float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 result = tex2D(uImage0, input.Texcoord.xy);
	return dot(result, mainColor) / dot(mainColor, mainColor) * mainColor;
}
float4 PixelShaderFunctionMagnifier(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage0, input.Texcoord.xy);
	if (!any(color))
		return color;
    // pos 就是中心了
	float2 pos = float2(0.5, 0.5);
    // offset 是中心到当前点的向量
	float2 offset = (input.Texcoord.xy - pos);
    // 因为长宽比不同进行修正
	float2 rpos = offset * float2(uScreenResolution.x / uScreenResolution.y, 1);
	float dis = length(rpos);
    // 向量长度缩短0.8倍
	return tex2D(uImage0, pos + offset * 0.8);
}
float4 PixelShaderFunctionDarkAndFlip(PSInput input) : COLOR0
{
	return tex2D(uImage0, float2(1, 1) - input.Texcoord.xy) * 0.5;
}

technique Technique1
{
	pass ColorScreen
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	pass Magnifier
	{
		PixelShader = compile ps_2_0 PixelShaderFunctionMagnifier();
	}
	pass DarkAndFlip
	{
		PixelShader = compile ps_2_0 PixelShaderFunctionDarkAndFlip();
	}
}