import axios from 'axios';

const { NEXT_PUBLIC_STABLE_DIFFUSION_API_KEY } = process.env;

export type StableDiffusionData = {
  modelId: string;
  prompt: string;
  negativePrompt: string;
  width: number;
  height: number;
  samples: number;
  numInferenceSteps: number;
  guidanceScale: number;
  multiLingual: boolean;
  selfAttention: boolean;
  embeddingsModel: string | null;
  loraModel: string | null;
};

type StableDiffusionResponse = {
  images: string[];
  error: string | null;
};

export async function generateImage({
  modelId,
  prompt,
  negativePrompt,
  width,
  height,
  samples,
  numInferenceSteps,
  guidanceScale,
  multiLingual,
  selfAttention,
  embeddingsModel,
  loraModel,
}: StableDiffusionData): Promise<StableDiffusionResponse> {
  const res = await axios.post(
    'https://stablediffusionapi.com/api/v4/dreambooth',
    {
      key: NEXT_PUBLIC_STABLE_DIFFUSION_API_KEY,
      model_id: modelId,
      prompt: prompt,
      negative_prompt: negativePrompt ? negativePrompt : null,
      width: width.toString(),
      height: height.toString(),
      samples: samples.toString(),
      num_inference_steps: numInferenceSteps.toString(),
      safety_checker: 'no',
      enhance_prompt: 'yes',
      seed: null,
      guidance_scale: guidanceScale,
      multi_lingual: multiLingual ? 'yes' : 'no',
      panorama: 'no',
      self_attention: selfAttention ? 'yes' : 'no',
      upscale: 'yes',
      embeddings_model: embeddingsModel,
      lora_model: loraModel,
      webhook: null,
      track_id: null,
    },
    {
      headers: { 'Content-Type': 'application/json' },
    },
  );

  if (res.data.status === 'success') {
    const images = res.data.output;
    console.log(images);
    return { images, error: null };
  } else if (res.data.status === 'processing') {
    console.log('processing');
    /* eslint-disable no-constant-condition */
    while (true) {
      /* eslint-disable no-constant-condition */
      const queuedRes = await axios.post(
        res.data.fetch_result,
        { key: NEXT_PUBLIC_STABLE_DIFFUSION_API_KEY },
        {
          headers: { 'Content-Type': 'application/json' },
        },
      );

      if (
        queuedRes.data.status === 'error' ||
        queuedRes.data.status === 'failed'
      ) {
        const error = queuedRes.data.message;
        console.error(error);
        return { images: [], error };
      }
      if (queuedRes.data.status === 'success') {
        const images = queuedRes.data.output;
        console.log(images);
        return { images, error: null };
      }
      await new Promise((resolve) => setTimeout(resolve, 3000));
      console.log('processing');
    }
  } else if (res.data.status === 'error' || res.data.status === 'failed') {
    const error = res.data.message;
    console.error(error);
    return { images: [], error };
  } else {
    const error = 'unknown error';
    console.error(error);
    return { images: [], error };
  }
}
