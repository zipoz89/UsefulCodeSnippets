
float customMax(float a, float b)
{
    return (a > b) ? a : b;
}

void GrayScale_float(float3 rgb,  out float3 grey)
{
    float max = customMax(customMax(rgb.x,rgb.y),rgb.z);
    
    grey = float3(max,max,max);
}