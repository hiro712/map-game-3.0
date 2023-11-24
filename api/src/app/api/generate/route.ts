import { getStableDiffusionData } from '@/utils/openai/chatgpt';
import {
  StableDiffusionData,
  generateImage,
} from '@/utils/stablediffusion/generate';
import { getImageURL, uploadImage } from '@/utils/supabase/storage';
import { downloadImageAsBuffer } from '@/utils/type/image';
import { reverseGeoCoder } from '@/utils/yahoo/reverseGeoCoder';
import { NextResponse } from 'next/server';

export async function POST(request: Request) {
  const body = await request.json();
  const lat = body.lat as number;
  const lng = body.lng as number;

  if (!lat) {
    console.error('lat is required');
    return NextResponse.json({ status: 'NOK', error: 'lat is required' });
  }
  if (!lng) {
    console.error('lng is required');
    return NextResponse.json({ status: 'NOK', error: 'lng is required' });
  }

  try {
    const { address, addressElements } = await reverseGeoCoder(lat, lng);
    console.log(address, addressElements);

    const { data: chatgptData, error: chatgptError } =
      await getStableDiffusionData(addressElements[0].Name);
    if (!!chatgptError || !chatgptData) {
      console.error('chatgpt error:', chatgptError);
      return NextResponse.json({ status: 'NOK', error: chatgptError });
    }

    const data: StableDiffusionData = {
      modelId: 'midjourney',
      prompt: chatgptData.prompt ? chatgptData.prompt : '',
      negativePrompt: chatgptData.negative_prompt
        ? chatgptData.negative_prompt
        : '',
      width: 512,
      height: 512,
      samples: 1,
      numInferenceSteps: 21,
      guidanceScale: 4,
      multiLingual: false,
      selfAttention: true,
      embeddingsModel: null,
      loraModel: null,
    };

    const { images, error: stablediffusionError } = await generateImage(data);
    if (stablediffusionError) {
      console.error('stablediffusion error:', stablediffusionError);
      return NextResponse.json({ status: 'NOK', error: stablediffusionError });
    }

    const image = images[0];

    console.log(image);
    const buffer = await downloadImageAsBuffer(image);
    const name = new Date().getTime().toString() + '.png';
    await uploadImage(buffer, name);
    const { data: urlData, error: urlError } = await getImageURL(name);
    if (!!urlError || !urlData) {
      console.error('url error:', urlError);
      return NextResponse.json({ status: 'NOK', error: urlError });
    }
    const url = urlData.signedUrl;
    console.log(url);

    return NextResponse.json({ status: 'OK', image: url });
  } catch (error) {
    console.error('unknown error:', error);
    return NextResponse.json({ status: 'NOK', error: error });
  }
}
