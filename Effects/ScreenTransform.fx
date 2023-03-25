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
//float4x4 TransformMatrix;
//bool useHeatMap;

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

//float4 PixelShaderFunction(PSInput input) : COLOR0
//{
//	float2 coords = input.Texcoord.xy;
//	float4 homoCoord = mul(float4(coords, 1, 0), TransformMatrix);
//	if (homoCoord.z == 0)
//		return float4(0, 0, 0, 0);
//	float2 current = homoCoord.xy / homoCoord.z;
//	float4 result = float4(0, 0, 0, 0);
//	if (current.x == saturate(current.x) && current.y == saturate(current.y))
//	{
//		result = tex2D(uImage0, current);
//	}
//	if (useHeatMap)
//	{
//		return tex2D(uImage1, float2(GetGrayValue(result.xyz), coords.y));
//	}
//	return result;
//}

//technique Technique1
//{
//	pass ScreenTransform
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
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
float4x4 TransformMatrix;
bool useHeatMap;

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

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float2 modifier = float2(uScreenResolution.x / uScreenResolution.y, 1);
	float2 coords = input.Texcoord.xy;
	float4 homoCoord = mul(float4((coords - float2(0.5, 0.5)) * modifier, 1, 0), TransformMatrix);
	if (homoCoord.z == 0)
		return float4(0, 0, 0, 0);
	float2 current = homoCoord.xy / homoCoord.z / modifier + float2(0.5, 0.5);
	float4 result = float4(0, 0, 0, 0);
	if (true)//current.x == saturate(current.x) && current.y == saturate(current.y)
	{
		result = tex2D(uImage0, current);
	}
	if (useHeatMap)
	{
		return tex2D(uImage1, float2(GetGrayValue(result.xyz), coords.y));
	}
	return result;
}

technique Technique1
{
	pass ScreenTransform
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}