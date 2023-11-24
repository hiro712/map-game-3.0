import { createClient } from '@supabase/supabase-js';

const { NEXT_PUBLIC_SUPABASE_PROJECT_URL, NEXT_PUBLIC_SUPABASE_API_KEY } =
  process.env;

const supabase = createClient(
  NEXT_PUBLIC_SUPABASE_PROJECT_URL as string,
  NEXT_PUBLIC_SUPABASE_API_KEY as string,
);

export async function uploadImage(image: Buffer, path: string) {
  const { data, error } = await supabase.storage
    .from('ipfs')
    .upload(path, image);
  if (error) {
    console.error(error);
  } else {
    console.log(data);
  }
}

export async function getImageURL(imagePath: string) {
  const { data, error } = await supabase.storage
    .from('ipfs')
    .createSignedUrl(imagePath, 3600 * 24 * 365 * 1);
  return { data, error };
}
