import axios from 'axios';

const { NEXT_PUBLIC_YAHOO_CLIENT_ID } = process.env;

type AddressElement = {
  Name: string;
  Kana: string;
  Level: string;
  Code: string;
};

export async function reverseGeoCoder(lat: number, lng: number) {
  const res = await axios.get(
    'https://map.yahooapis.jp/geoapi/V1/reverseGeoCoder',
    {
      params: {
        lat,
        lon: lng,
        datum: 'wgs',
        output: 'json',
        appid: NEXT_PUBLIC_YAHOO_CLIENT_ID,
      },
    },
  );
  const address: string = res.data.Feature[0].Property.Address;
  const addressElements: AddressElement[] =
    res.data.Feature[0].Property.AddressElement;
  const data = { address, addressElements };
  return data;
}
