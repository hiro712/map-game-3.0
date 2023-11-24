import { ThirdwebStorage } from '@thirdweb-dev/storage';

const { NEXT_PUBLIC_THIRDWEB_CLIENT_SECRET } = process.env;
console.log(
  'NEXT_PUBLIC_THIRDWEB_CLIENT_SECRET',
  NEXT_PUBLIC_THIRDWEB_CLIENT_SECRET,
);

const storage = new ThirdwebStorage({
  secretKey: NEXT_PUBLIC_THIRDWEB_CLIENT_SECRET,
});

export async function uploadJson(json: any) {
  try {
    const uri = await storage.upload(json); // ipfs://QmWgbcjKWCXhaLzMz4gNBxQpAHktQK6MkLvBkKXbsoWEEy/0
    const url = storage.resolveScheme(uri); // https://ipfs.thirdwebstorage.com/ipfs/QmWgbcjKWCXhaLzMz4gNBxQpAHktQK6MkLvBkKXbsoWEEy/0
    const data = await storage.downloadJSON(uri);
    const res = { uri, url, data, error: null };
    return res;
  } catch (error) {
    return { uri: null, url: null, data: null, error };
  }
}
