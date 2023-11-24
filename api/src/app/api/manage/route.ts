import prisma from '@/configs/prisma/prisma';
import { Manage } from '@prisma/client';
import { NextResponse } from 'next/server';

type ResponseManage = {
  id: number;
  createdAt: string;
  updatedAt: string;
  address: string;
  lat: number;
  lng: number;
  status: number;
  image?: string;
};

function manage2Response(manage: Manage): ResponseManage {
  const { id, created_at, updated_at, address, lat, lng, status, image } =
    manage;
  return {
    id,
    createdAt: created_at.toISOString(),
    updatedAt: updated_at.toISOString(),
    address,
    lat,
    lng,
    status,
    image: image ? image : undefined,
  };
}

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url);
  const address = searchParams.get('address') as string;

  const manages: Manage[] = await prisma.manage.findMany({
    where: {
      address,
    },
  });

  const data = manages.map(manage2Response);

  return NextResponse.json(data);
}

export async function POST(request: Request) {
  const body = await request.json();
  const { address, lat, lng, status } = body;

  await prisma.manage.create({
    data: {
      address: address as string,
      lat: lat as number,
      lng: lng as number,
      status: status as number,
    },
  });

  const manages: Manage[] = await prisma.manage.findMany({
    where: {
      address,
    },
  });

  const data = manages.map(manage2Response);

  return NextResponse.json(data);
}

export async function PUT(request: Request) {
  const body = await request.json();
  const { id, address, status, image } = body;

  await prisma.manage.update({
    where: {
      id: id as number,
    },
    data: {
      address: address as string,
      status: status as number,
      image: image as string,
    },
  });

  const manages: Manage[] = await prisma.manage.findMany({
    where: {
      address,
    },
  });

  const data = manages.map(manage2Response);

  return NextResponse.json(data);
}

export async function DELETE(request: Request) {
  const { searchParams } = new URL(request.url);
  const id = searchParams.get('id') as string;
  const address = searchParams.get('address') as string;

  await prisma.manage.delete({
    where: {
      id: Number(id),
    },
  });

  const manages: Manage[] = await prisma.manage.findMany({
    where: {
      address,
    },
  });

  const data = manages.map(manage2Response);

  return NextResponse.json(data);
}
