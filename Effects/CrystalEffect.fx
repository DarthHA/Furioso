sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);


float threshold;             //溶解临界值，范围~1
float fadeout;               //0为完全消失，1为不消失
float4x4 uTransform;

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

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 coords = float2(input.Texcoord.x,input.Texcoord.y);
    float4 color = tex2D(uImage0, coords);
    color = color * input.Texcoord.z;
	float4 n = tex2D(uImage1, coords);
    if (!any(color))
        return color;
    float4  inter_color = float4(1.0, 1.0, 1.0, 1.0);        //溶解边缘颜色
    float _fadeout = fadeout;
    if (color.a > 0.0) 
    {
        float diff = n.r - _fadeout;
        if (diff > threshold) 
        {
            color.a = 0.0;
        }
        else if (diff < threshold && diff > 0.0) 
        {
            float a = n.r * diff / threshold;
            a *= 4;
            color = color * (1.0 - a) + inter_color * a;
        }
        else 
        {
            color = color * fadeout * tex2D(uImage2, coords);
        }
    }
    if (color.r > 0.9) return float4(0, 0, 0, 0);
    return color;
}

technique Technique1
{
	pass CrystalEffect
	{
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}