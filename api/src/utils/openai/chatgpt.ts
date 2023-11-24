import { OpenAI } from 'openai';
import { ChatgptPrompt } from './prompt';

const { NEXT_PUBLIC_OPENAI_API_KEY } = process.env;

type ChatGPTResponse = {
  key?: string;
  prompt?: string;
  negative_prompt?: string;
  width?: number;
  height?: number;
  samples?: number;
  num_inference_steps?: number;
  safety_checker?: string;
  enhance_prompt?: string;
  seed?: number;
  guidance_scale?: number;
  multi_lingual?: string;
  panorama?: string;
  self_attention?: string;
  upscale?: string;
  embeddings_model?: string;
  webhook?: string;
  track_id?: string;
};

export async function getStableDiffusionData(place: string) {
  const gptPrompt = ChatgptPrompt + '与える地名:「' + place + '」';
  const openai = new OpenAI({
    apiKey: NEXT_PUBLIC_OPENAI_API_KEY as string,
    dangerouslyAllowBrowser: true,
  });

  const res = await openai.chat.completions
    .create({
      model: 'gpt-3.5-turbo',
      messages: [{ role: 'user', content: gptPrompt }],
    })
    .then((response) => {
      if (response.choices[0].message.content) {
        const answer = JSON.parse(
          response.choices[0].message.content,
        ) as ChatGPTResponse;
        console.log(answer);
        return { data: answer, error: null };
      }
    })
    .catch((error) => {
      console.error(error);
      return { data: null, error: error };
    });
  if (res === undefined) {
    return { data: null, error: 'error' };
  } else {
    return res;
  }
}
