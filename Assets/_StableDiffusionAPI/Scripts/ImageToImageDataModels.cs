using System;

namespace StableDiffusionAPI
{
    [Serializable]
    public struct ImageToImageArgs
    {
        public string[] init_images;
        public string prompt;
        public int width;
        public int height;
        public int steps;
        public int seed;
        public float cfg_scale;
        public float image_cfg_scale;
        public float denoising_strength;
        public string sampler_index;
        public string negative_prompt;
        
        
        public static readonly ImageToImageArgs Default = new ImageToImageArgs
        {
            init_images = new string[1],
            prompt = "",
            steps = 36,
            seed = UnityEngine.Random.Range(0, Int32.MaxValue),
            cfg_scale = 7,
            image_cfg_scale = 7f,
            denoising_strength = 0.74f,
            sampler_index = "Euler a",
            negative_prompt = "boring background , amputee, blurry, boring, close-up, details are low, distorted details ,eerie , grains, grainy, low contrast, low quality ,low resolution  ,multiple angles , multiple views , overexposed , oversaturated , plain , plain background , simple background , standard , surreal , unattractive , uncreative , underexposed , unprofessional  , ,Autograph , Bad anatomy ,Bad illustration ,Bad proportions ,Beyond the borders ,Blank background ,Blurry ,Body out of frame , Boring ,background , Branding ,Cropped ,Cut off ,Deformed ,Disfigured ,Dismembered ,Disproportioned ,Distorted ,Draft ,Duplicate ,Duplicated features ,Extra arms ,Extra fingers ,Extra hands ,Extra legs ,Extra limbs ,Fault ,Flaw ,Fused fingers ,Grains ,Grainy ,Gross proportions ,Hazy ,Identifying mark ,Improper scale ,Incorrect , physiology ,Incorrect ratio ,Indistinct ,Kitsch ,Logo ,Long neck ,Low quality ,Low resolution ,Macabre ,Malformed ,Mark ,Misshapen ,Missing arms ,Missing fingers, Missing hands ,Missing legs ,Mistake ,Morbid , Mutated hands ,Mutation ,Mutilated ,Off-screen , Out of frame ,Outside the picture ,Pixelated ,Poorly drawn face, Poorly drawn feet ,Poorly drawn hands ,Printed words ,Render ,Repellent ,Replicate ,Reproduce ,Revolting dimensions ,Script ,Shortened ,Sign ,Signature , Split image ,Squint ,Storyboard ,Text ,Tiling ,Trimmed ,Ugly ,Unfocused ,Unattractive ,Unnatural pose ,Unreal engine ,Unsightly ,Watermark ,Written language , Poorly drawn planet, letter , language , number",
        };
    }

    
    [Serializable]
    public struct ImageToImageResponse
    {
        public string[] images;
    }
}