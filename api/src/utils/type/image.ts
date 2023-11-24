import axios from 'axios';
import { promises as fs } from 'fs';

// URLを受け取り、Bufferを返す非同期関数
export async function downloadImageAsBuffer(url: string): Promise<Buffer> {
  const response = await axios({
    url,
    method: 'GET',
    responseType: 'arraybuffer', // バイナリデータとしてレスポンスを受け取る
  });

  return Buffer.from(response.data);
}

// Bufferをファイルとして保存する関数
async function saveBufferToFile(
  buffer: Buffer,
  filename: string,
): Promise<void> {
  await fs.writeFile(filename, buffer);
}

// URLから画像をダウンロードしてファイルに保存する関数
export async function downloadImageToFile(
  url: string,
  filename: string,
): Promise<void> {
  try {
    const buffer = await downloadImageAsBuffer(url);
    await saveBufferToFile(buffer, filename);
    console.log(`Image has been downloaded and saved as ${filename}`);
  } catch (error) {
    console.error('An error occurred:', error);
  }
}
