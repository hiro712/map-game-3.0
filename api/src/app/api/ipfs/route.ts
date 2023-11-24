import { uploadJson } from '@/utils/thirdweb/storage';
import { NextResponse } from 'next/server';

export async function POST(request: Request) {
  const body = await request.json();
  const jsonString: string | null = body.json;

  if (!jsonString) {
    console.error('required json');
    return NextResponse.json({ status: 'NOK', error: 'required json' });
  }

  const json = JSON.parse(jsonString);
  const { uri, url, data, error } = await uploadJson(json);
  if (error) {
    console.error(error);
    return NextResponse.json({ status: 'NOK', error });
  }
  return NextResponse.json({ status: 'OK', uri, url, data });
}
