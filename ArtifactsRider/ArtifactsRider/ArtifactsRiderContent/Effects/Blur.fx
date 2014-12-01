sampler ScreenS; 

//#define Param 0.05

float Param;

// TODO: add effect parameters here.
float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{
    float4 c = tex2D(ScreenS, texCoord);
    c += tex2D(ScreenS, texCoord - float2(Param,Param));
    c += tex2D(ScreenS, texCoord - float2(0,Param));
    c += tex2D(ScreenS, texCoord - float2(-Param,Param));
    
    c += tex2D(ScreenS, texCoord - float2(Param,0));
    c += tex2D(ScreenS, texCoord - float2(-Param,0));
    
    c += tex2D(ScreenS, texCoord - float2(Param,-Param));
    c += tex2D(ScreenS, texCoord - float2(0,-Param));
    c += tex2D(ScreenS, texCoord - float2(-Param,-Param));
    
    return c /9;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
